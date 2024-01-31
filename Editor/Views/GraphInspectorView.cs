using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class GraphInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<GraphInspectorView, UxmlTraits>{}
        
        private readonly string uxmlName = "GraphInspector.uxml";

        // Data
        private QuestSO quest;
        private NodeSO nodeSelected;
        
        // Visual elements
        private QuestInspectorView questInspectorView;
        private NodeInspectorView nodeInspectorView;

        public GraphInspectorView()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(QuestGraphEditorWindow.RelativePath, uxmlName);
            uxml.CloneTree(this);

            style.flexGrow = 1;

            questInspectorView = this.Q<QuestInspectorView>();
            nodeInspectorView = this.Q<NodeInspectorView>();

            questInspectorView.HideOpenGraphButton();
            ShowQuestInspector();
        }

        public void BindQuest(QuestSO quest)
        {
            questInspectorView.BindQuest(quest);
        }

        public void BindNode(NodeSO nodeData)
        {
            nodeSelected = nodeData;
            
            if (nodeData is TaskNodeSO taskNodeData)
            {
                nodeInspectorView.BindTask(taskNodeData.task);
            }
            else
            {
                nodeInspectorView.UnbindTask();
            }
            
            ShowNodeInspector();
        }

        private void ShowQuestInspector()
        {
            nodeInspectorView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            questInspectorView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }
        
        private void ShowNodeInspector()
        {
            questInspectorView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None); 
            nodeInspectorView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }
        
        public void UnbindNode()
        {
            nodeSelected = null;
            nodeInspectorView.UnbindTask();
            
            ShowQuestInspector();
        }
    }
}