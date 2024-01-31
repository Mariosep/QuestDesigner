using UnityEngine;

namespace QuestDesigner.Editor
{
    public static class NodeFactory
    {
        public static NodeSO CreateNode(NodeType type, Vector2 position)
        {
            switch (type)
            {
                case NodeType.Start:
                    StartNodeSO startNode = ScriptableObject.CreateInstance<StartNodeSO>();
                    startNode.Init(position);
                    return startNode;

                case NodeType.Complete:
                    CompleteNodeSO completeNode = ScriptableObject.CreateInstance<CompleteNodeSO>();
                    completeNode.Init(position);
                    return completeNode;

                case NodeType.Fail:
                    FailNodeSO failNode = ScriptableObject.CreateInstance<FailNodeSO>();
                    failNode.Init(position);
                    return failNode;

                case NodeType.Task:
                    TaskNodeSO taskNode = ScriptableObject.CreateInstance<TaskNodeSO>();
                    taskNode.Init(position);
                    return taskNode;

                case NodeType.Branch:
                    BranchNodeSO branchNode = ScriptableObject.CreateInstance<BranchNodeSO>();
                    branchNode.Init(position);
                    return branchNode;

                default:
                    return null;
            }
        }
    }
}