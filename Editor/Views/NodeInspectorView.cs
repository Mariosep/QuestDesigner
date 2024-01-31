using Blackboard;
using Blackboard.Editor.Commands;
using Blackboard.Editor.Requirement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class NodeInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<NodeInspectorView, UxmlTraits>{}
        
        private readonly string uxmlName = "TaskInspector.uxml";

        private TextField taskNameTextField;
        private TextField descriptionField;
        private VisualElement taskInspectorContent;
        private RequirementsListView requirementsListView;
        private ListView nextTasksListView;
        private CommandListView onStartCommandListView;
        private CommandListView onCompleteCommandListView;

        private TaskSO taskData;

        public NodeInspectorView()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(QuestGraphEditorWindow.RelativePath, uxmlName);
            uxml.CloneTree(this);

            GetReferences();

            taskInspectorContent.visible = false;
        
            nextTasksListView.makeItem = NextTasksMakeItem;
            nextTasksListView.bindItem = (element, i) => NextTasksBindItem(element, taskData.nextSteps[i]);
            nextTasksListView.bindingPath = "nextTasks";

            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            descriptionField.RegisterCallback<FocusOutEvent>(e =>
            {
                descriptionField.value = descriptionField.value.Trim();
                taskData.description = descriptionField.value;
            });
        }

        private void GetReferences()
        {
            taskInspectorContent = this.Q<VisualElement>("task-inspector__content");
            taskNameTextField = this.Q<TextField>("task-name__text-field");
            descriptionField = this.Q<TextField>("task-description__text-field");
            requirementsListView = this.Q<RequirementsListView>();
            nextTasksListView = this.Q<ListView>("task-next-tasks__list-view");
            onStartCommandListView = this.Q<CommandListView>("on-start-task");
            onCompleteCommandListView = this.Q<CommandListView>("on-complete-task");
        }

        public void BindTask(TaskSO taskData)
        {
            this.Unbind();

            this.taskData = taskData;
            this.Bind(new SerializedObject(taskData));

            BindName();
            BindRequirements();
            BindOnStartCommandList();
            BindOnCompleteCommandList();

            taskInspectorContent.visible = true;
        }

        // TODO: Refactor
        private void BindName()
        {
            taskNameTextField.value = taskData.taskName;
            taskNameTextField.RegisterCallback<FocusOutEvent>(e =>
            {
                taskNameTextField.value = taskNameTextField.value.Trim();
                bool isValid = BlackboardValidator.ValidateFormatName(taskNameTextField.value, taskData.taskName);

                if (isValid)
                    taskData.SetName(taskNameTextField.value);
                else
                    taskNameTextField.value = taskData.taskName;
            });
        }

        private void BindRequirements()
        {
            requirementsListView.SaveAsSubAssetOf(QuestGraphEditorWindow.questGraph);
            requirementsListView.PopulateView(taskData.requirements);
        }
        
        private void BindOnStartCommandList()
        {
            onStartCommandListView.SaveAsSubAssetOf(QuestGraphEditorWindow.questGraph);
            onStartCommandListView.PopulateView(taskData.onStart);
        }
        
        private void BindOnCompleteCommandList()
        {
            onCompleteCommandListView.SaveAsSubAssetOf(QuestGraphEditorWindow.questGraph);
            onCompleteCommandListView.PopulateView(taskData.onComplete);
        }

        public void UnbindTask()
        {
            this.Unbind();
            this.taskData = null;

            taskInspectorContent.visible = false;
        }

        private VisualElement NextTasksMakeItem()
        {
            var nextStepObjectField = new ObjectField();
            nextStepObjectField.objectType = typeof(StepSO);

            return nextStepObjectField;
        }

        private void NextTasksBindItem(VisualElement element, StepSO nextStep)
        {
            var nextStepObjectField = element as ObjectField;
            nextStepObjectField.value = nextStep;
        }
    }
}