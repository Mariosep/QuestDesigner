using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<QuestGraphView, UxmlTraits>{}
        
        private readonly string styleSheetName = "QuestEditor.uss";

        public Action<NodeView> onNodeSelected;
        public Action<NodeView> onNodeUnselected;

        public QuestGraphSO questGraph;
        
        private QuestGraphDebugHandler _questGraphDebugHandler;

        private QuestGraphEditorWindow _graphEditorWindow;
        private QuestGraphSearchProvider searchProvider;

        private float graphMiddleHeight;

        public List<NodeView> nodesToCopy = new List<NodeView>();
        public List<Edge> edgesToCopy = new List<Edge>();

        public QuestGraphView()
        {
            Insert(0, new GridBackground());

            style.flexGrow = 1;

            AddManipulators();
            AddSearchWindow();

            _questGraphDebugHandler = new QuestGraphDebugHandler(this);

            var styleSheet = UIToolkitLoader.LoadStyleSheet(QuestGraphEditorWindow.RelativePath, styleSheetName);
            styleSheets.Add(styleSheet);
        }

        public void PopulateView(QuestGraphSO questGraph)
        {
            this.questGraph = questGraph;

            // TODO: Refactor subscription to events
            UnregisterCallbacks();

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            float graphHeight = resolvedStyle.height;
            graphMiddleHeight = graphHeight / 2;

            // Create root node if is not registered
            if (questGraph.startNode == null)
            {
                questGraph.startNode =
                    questGraph.CreateNode(NodeType.Start, new Vector2(50, graphMiddleHeight - 30)) as StartNodeSO;
                EditorUtility.SetDirty(questGraph);
                AssetDatabase.SaveAssets();
            }

            if (questGraph.completeNode == null)
            {
                questGraph.completeNode =
                    questGraph.CreateNode(NodeType.Complete,
                        new Vector2(600, graphMiddleHeight - 30)) as CompleteNodeSO;
                EditorUtility.SetDirty(questGraph);
                AssetDatabase.SaveAssets();
            }

            if (questGraph.quest.failable && questGraph.failNode == null)
            {
                questGraph.failNode =
                    questGraph.CreateNode(NodeType.Fail, new Vector2(600, graphMiddleHeight - 30 + 300)) as FailNodeSO;
                EditorUtility.SetDirty(questGraph);
                AssetDatabase.SaveAssets();
            }

            PopulateGraphElements();

            RegisterCallbacks();
        }

        public void SetEditorWindow(QuestGraphEditorWindow graphEditorWindow)
        {
            _graphEditorWindow = graphEditorWindow;
        }

        public void EnableDebugMode(QuestJournalSO questJournal)
        {
            _questGraphDebugHandler.EnableDebugMode(questJournal);
        }

        public void DisableDebugMode()
        {
            _questGraphDebugHandler.DisableDebugMode();
        }

        private void RegisterCallbacks()
        {
            Undo.undoRedoPerformed += OnUndoRedo;
            serializeGraphElements += CopyNode;

            questGraph.quest.onFailableValueChanged += OnFailableValueChanged;

            RegisterCallback<MouseUpEvent>(CheckPasteAction);
            RegisterCallback<KeyDownEvent>(DisableRedoAction);
            RegisterCallback<DetachFromPanelEvent>(e => UnregisterCallbacks());
        }

        private void UnregisterCallbacks()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
            serializeGraphElements -= CopyNode;

            questGraph.quest.onFailableValueChanged -= OnFailableValueChanged;

            UnregisterCallback<KeyDownEvent>(DisableRedoAction);
            UnregisterCallback<MouseUpEvent>(CheckPasteAction);
        }

        #region Populate

        protected void PopulateGraphElements()
        {
            // Creates node views
            foreach (NodeSO node in questGraph.nodes)
            {
                CreateNodeView(node);
            }

            // Create edges
            foreach (NodeSO node in questGraph.nodes)
            {
                foreach (PortSO outputPort in node.outputPorts)
                {
                    List<NodeSO> targetNodes = outputPort.targetNodes;

                    foreach (NodeSO targetNode in targetNodes)
                    {
                        NodeView targetNodeView = FindNodeView(targetNode);

                        var originPortView = GetPortByGuid(outputPort.id);
                        Edge edge = originPortView.ConnectTo(targetNodeView.inputPortsList[0]);
                        AddElement(edge);
                    }
                }
            }
        }

        protected void AddManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            QuestGraphContextualMenu.Populate(this, evt);
        }

        private void AddSearchWindow()
        {
            if (searchProvider == null)
            {
                searchProvider = ScriptableObject.CreateInstance<QuestGraphSearchProvider>();
                searchProvider.Initialize(this);
            }

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchProvider);
        }

        #endregion

        #region Modify

        public NodeView CreateNode(NodeType type, Vector2 position)
        {
            NodeSO node = questGraph.CreateNode(type, position);
            return CreateNodeView(node);
        }

        protected NodeView CreateNodeView(NodeSO node)
        {
            NodeView nodeView = NodeViewFactory.CreateNodeView(node);
            nodeView.onNodeSelected = OnNodeSelected;
            nodeView.onNodeUnselected = onNodeUnselected;
            AddElement(nodeView);

            return nodeView;
        }

        private void OnNodeSelected(NodeView nodeView)
        {
            if (selection.Count == 1)
                onNodeSelected?.Invoke(nodeView);
        }

        private string CopyNode(IEnumerable<GraphElement> elements)
        {
            nodesToCopy.Clear();
            edgesToCopy.Clear();

            foreach (GraphElement e in elements)
            {
                if (e is NodeView nodeView)
                {
                    nodesToCopy.Add(nodeView);
                }
                else if (e is Edge edge)
                {
                    edgesToCopy.Add(edge);
                }
            }

            return "Selection copied";
        }

        public void PasteNode(Vector2 position)
        {
            Vector2 middlePosition;
            Vector2 average = Vector2.zero;

            foreach (NodeView nodeView in nodesToCopy)
            {
                average += nodeView.GetPosition().position;
            }

            middlePosition = average / nodesToCopy.Count;

            ClearSelection();

            var undoGroup = Undo.GetCurrentGroup();
            Undo.RecordObject(questGraph, "Duplicate nodes");

            List<NodeView> pastedNodeViewList = new List<NodeView>();
            foreach (NodeView nodeView in nodesToCopy)
            {
                Vector2 offset = nodeView.GetPosition().position - middlePosition;

                NodeView pastedNodeView = DuplicateNode(nodeView.node, position + offset);
                pastedNodeViewList.Add(pastedNodeView);

                Undo.RegisterCreatedObjectUndo(pastedNodeView.node, "Duplicate nodes");
            }

            foreach (Edge edge in edgesToCopy)
            {
                NodeView copiedInputNode = edge.input.node as NodeView;
                NodeView copiedOutputNode = edge.output.node as NodeView;

                int inputNodeIndex = nodesToCopy.IndexOf(copiedInputNode);
                int outputNodeIndex = nodesToCopy.IndexOf(copiedOutputNode);

                if (inputNodeIndex == -1 || outputNodeIndex == -1)
                    return;

                NodeView pastedInputNode = pastedNodeViewList[inputNodeIndex];
                NodeView pastedOutputNode = pastedNodeViewList[outputNodeIndex];

                Port originPastedPort = pastedOutputNode.GetOutputPortByName(edge.output.portName);

                questGraph.AddChild(originPastedPort.viewDataKey, pastedOutputNode.node, pastedInputNode.node);

                Edge pastedEdge = originPastedPort.ConnectTo(pastedInputNode.inputPortsList[0]);
                AddElement(pastedEdge);

                Undo.RecordObject(pastedOutputNode.node, "Duplicate nodes");

                AddToSelection(pastedEdge);
            }

            Undo.CollapseUndoOperations(undoGroup);
        }

        public NodeView DuplicateNode(NodeSO node, Vector2 position)
        {
            if (node is not TaskNodeSO)
                return null;

            NodeSO clonedNode = node.Clone();

            clonedNode.position = new Rect(position, Vector2.zero);
            clonedNode.RemoveAllOutputNodes();
            questGraph.AddNode(clonedNode);

            NodeView clonedNodeView = CreateNodeView(clonedNode);

            AddToSelection(clonedNodeView);

            return clonedNodeView;

        }

        #endregion

        #region Events

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    if (element is NodeView nodeView)
                    {
                        if (nodesToCopy.Contains(nodeView))
                            nodesToCopy.Remove(nodeView);

                        questGraph.DeleteNode(nodeView.node);
                    }

                    if (element is Edge edge)
                        OnEdgeRemoved(edge);
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                foreach (Edge edge in graphViewChange.edgesToCreate)
                    OnEdgeAdded(edge);
            }

            return graphViewChange;
        }

        private void OnEdgeAdded(Edge edge)
        {
            NodeView parentNodeView = edge.output.node as NodeView;
            NodeView childNodeView = edge.input.node as NodeView;

            string originPortId = edge.output.viewDataKey;

            questGraph.AddChild(originPortId, parentNodeView.node, childNodeView.node);
        }

        private void OnEdgeRemoved(Edge edge)
        {
            NodeView parentNodeView = edge.output.node as NodeView;
            NodeView childNodeView = edge.input.node as NodeView;

            string originPortId = edge.output.viewDataKey;

            questGraph.RemoveChild(originPortId, parentNodeView.node, childNodeView.node);
        }

        public void OnFailableValueChanged(bool value)
        {
            if (value)
            {
                if (questGraph.failNode == null)
                {
                    questGraph.failNode =
                        questGraph.CreateNode(NodeType.Fail, new Vector2(600, graphMiddleHeight - 30 + 300)) as
                            FailNodeSO;
                    EditorUtility.SetDirty(questGraph);
                    AssetDatabase.SaveAssets();

                    CreateNodeView(questGraph.failNode);
                }
            }
            else
            {
                if (questGraph.failNode != null)
                {
                    foreach (Node node in nodes)
                    {
                        if (node is FailNodeView failNodeView)
                        {
                            DeleteElements(new[] { (GraphElement)failNodeView });
                        }
                    }

                    questGraph.failNode = null;
                }
            }
        }

        private void CheckPasteAction(MouseUpEvent evt)
        {
            if (Keyboard.current.altKey.isPressed && selection.Count > 0)
            {
                nodesToCopy.Clear();
                edgesToCopy.Clear();

                foreach (ISelectable selectable in selection)
                {
                    if (selectable is NodeView nodeView)
                    {
                        nodesToCopy.Add(nodeView);
                    }

                    if (selectable is Edge edge)
                    {
                        edgesToCopy.Add(edge);
                    }
                }

                if (nodesToCopy.Count > 0)
                {
                    Vector2 position = GetLocalMousePosition(evt.mousePosition);
                    PasteNode(position);
                }
            }
        }

        private void OnUndoRedo()
        {
            PopulateView(questGraph);
            AssetDatabase.SaveAssets();
        }

        private void DisableRedoAction(KeyDownEvent evt)
        {
            if (evt.modifiers == EventModifiers.Control && evt.keyCode == KeyCode.Y)
            {
                evt.StopImmediatePropagation();
                evt.StopPropagation();
            }
        }

        #endregion

        #region Utils

        public NodeView FindNodeView(NodeSO node)
        {
            return GetNodeByGuid(node.id) as NodeView;
        }

        public NodeView FindNodeView(string id)
        {
            return GetNodeByGuid(id) as NodeView;
        }

        public Port FindPortView(string id)
        {
            return GetPortByGuid(id);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction && endPort.node != startPort.node
            ).ToList();
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= _graphEditorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        #endregion
    }
}