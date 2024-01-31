using Blackboard.Editor.Requirement;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class TaskNodeViewContainer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TaskNodeViewContainer, UxmlTraits>{}
        
        private readonly string uxmlName = "TaskNodeViewContainer.uxml";

        private SimplifiedRequirementsListView requirementsListView;

        public TaskNodeViewContainer()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(QuestGraphEditorWindow.RelativePath, uxmlName);
            uxml.CloneTree(this);

            requirementsListView = this.Q<SimplifiedRequirementsListView>();
        }

        public void PopulateView(TaskNodeSO node)
        {
            //requirementsListView.SaveAsSubAssetOf(QuestGraphEditorWindow.questGraph);
            requirementsListView.PopulateView(node.task.requirements);
        }
        
        public void OnNodeEntered(Task task)
        {
            requirementsListView.EnableDebugMode(task.requirements);
        }
        
        public void OnNodeExited()
        {
            requirementsListView.DisableDebugMode();
        }
    }
}