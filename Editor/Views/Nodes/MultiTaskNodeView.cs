using QuestDesigner;

namespace QuestDesigner.Editor
{
    public class MultiTaskNodeView : NodeView
    {
        public MultiTaskNodeView(MultiTaskNodeSO node) : base(node, QuestGraphEditorWindow.RelativePath + "/UXML/Nodes/NodeView.uxml")
        {
            title = "Multi Task"; //node.task.taskName;

            //node.task.onNameChanged += OnTaskNameChanged;

            /*var taskNodeViewContainer = new TaskNodeViewContainer();
        taskNodeViewContainer.PopulateView(node);
        
        extensionContainer.Add(taskNodeViewContainer);
        RefreshExpandedState();*/
        }

        private void OnTaskNameChanged(string taskName)
        {
            title = taskName;
        }
    }
}