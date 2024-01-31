namespace QuestDesigner.Editor
{
    public class BranchNodeView : NodeView
    {
        public BranchNodeView(BranchNodeSO node) : base(node, QuestGraphEditorWindow.RelativePath + "/UXML/Nodes/NodeView.uxml")
        {
            title = "Branch";

            var conditionNodeViewContainer = new BranchNodeViewContainer();
            conditionNodeViewContainer.PopulateView(node);

            extensionContainer.Add(conditionNodeViewContainer);
            RefreshExpandedState();
        }
    }

}