using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public class QuestGraphSO : ScriptableObject
    {
        public QuestSO quest;

        public string id;

        public StartNodeSO startNode;
        public CompleteNodeSO completeNode;
        public FailNodeSO failNode;
        public List<NodeSO> nodes = new List<NodeSO>();

        public void Init()
        {
            id = GUID.Generate().ToString();
            name = $"questGraph-{id}";

            quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.Init(id);
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public NodeSO CreateNode(NodeType type, Vector2 position)
        {
            EditorUtility.SetDirty(this);

            NodeSO node = NodeFactory.CreateNode(type, position);

            Undo.RecordObject(this, "Create node");
            nodes.Add(node);

            if (!Application.isPlaying)
                node.SaveAs(this);

            Undo.RegisterCreatedObjectUndo(node, "Create node");
            AssetDatabase.SaveAssets();
            return node;
        }

        public void AddNode(NodeSO node)
        {
            nodes.Add(node);
            node.SaveAs(this);
        }

        public void DeleteNode(NodeSO node)
        {
            Undo.RecordObject(this, "Delete Node");
            nodes.Remove(node);

            Undo.DestroyObjectImmediate(node);

            AssetDatabase.SaveAssets();
        }

        // TODO: Change method name
        // TODO: Move method to NodeSO 
        public void AddChild(string originPortId, NodeSO parent, NodeSO child)
        {
            Undo.RecordObject(parent, "Node (Add child)");
            parent.AddOutputNode(originPortId, child);
            EditorUtility.SetDirty(parent);

            // TODO: Refactor to be more readable
            if (parent is StartNodeSO)
            {
                quest.initialStep = child.step;
            }
            else
            {
                if (child is CompleteNodeSO)
                {
                    parent.step.completeQuest = true;
                }
                else if (child is FailNodeSO)
                {
                    parent.step.failQuest = true;
                }
            }
        }

        // TODO: Change method name
        public void RemoveChild(string originPortName, NodeSO parent, NodeSO child)
        {
            Undo.RecordObject(parent, "Node (Remove child)");
            parent.RemoveOutputNode(originPortName, child);
            EditorUtility.SetDirty(parent);

            if (parent is StartNodeSO && child is TaskNodeSO taskNode)
            {
                quest.initialStep = null;
            }
            else if (parent is TaskNodeSO lastTaskNode)
            {
                if (child is CompleteNodeSO)
                {
                    lastTaskNode.task.completeQuest = false;
                }
                else if (child is FailNodeSO)
                {
                    lastTaskNode.task.failQuest = false;
                }
            }
        }
    }
}