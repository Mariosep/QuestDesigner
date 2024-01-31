namespace QuestDesigner.Editor
{
    public static class NodeViewFactory
    {
        public static NodeView CreateNodeView(NodeSO node)
        {
            return node switch
            {
                StartNodeSO startNodeSo => new StartNodeView(startNodeSo),
                CompleteNodeSO completeNodeSo => new CompleteNodeView(completeNodeSo),
                FailNodeSO failNodeSo => new FailNodeView(failNodeSo),
                TaskNodeSO taskNodeSo => new TaskNodeView(taskNodeSo),
                BranchNodeSO conditionNodeSo => new BranchNodeView(conditionNodeSo),
                _ => null
            };
        }
    }
}