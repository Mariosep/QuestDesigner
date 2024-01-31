namespace QuestDesigner.Editor
{
    public class TaskNodeView : NodeView
    {
        private TaskNodeViewContainer taskNodeViewContainer;
        
        public TaskNodeView(TaskNodeSO node) : base(node, QuestGraphEditorWindow.RelativePath + "/UXML/Nodes/NodeView.uxml")
        {
            title = node.task.taskName;

            node.task.onNameChanged += OnTaskNameChanged;

            taskNodeViewContainer = new TaskNodeViewContainer();
            taskNodeViewContainer.PopulateView(node);

            extensionContainer.Add(taskNodeViewContainer);
            RefreshExpandedState();
        }

        private void OnTaskNameChanged(string taskName)
        {
            title = taskName;
        }

        protected override void CreateInputPorts()
        {
            base.CreateInputPorts();
            inputPortsList[0].portName = "Previous";
        }

        protected override void CreateOutputPorts()
        {
            base.CreateOutputPorts();
            outputPortsList[0].portName = "Next";
        }

        public override void OnNodeEntered(Step step)
        {
            taskNodeViewContainer.OnNodeEntered((Task)step);
        }

        public override void OnNodeExited()
        {
            taskNodeViewContainer.OnNodeExited();
        }
    }
}