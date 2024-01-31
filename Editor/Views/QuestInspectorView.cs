using Blackboard.Editor.Commands;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestInspectorView, UxmlTraits>{}
        
        private readonly string uxmlName = "QuestInspector.uxml";

        private QuestSO quest;

        private VisualElement questInspectorContent;

        //private TextField idTextField;
        private TextField nameField;
        private TextField descriptionField;
        private Toggle failableToggle;
        private QuestDropdown nextQuestDropdown;
        private CommandListView onStartCommandListView;
        private CommandListView onCompleteCommandListView;

        private Button openQuestGraphButton;

        public QuestInspectorView()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(QuestGraphEditorWindow.RelativePath, uxmlName);
            uxml.CloneTree(this);

            GetReferences();

            nextQuestDropdown.SetName("Next Quest");
            nextQuestDropdown.AddNoneOption();
            nextQuestDropdown.onQuestSelected += SetNextQuest;

            RegisterCallbacks();
        }

        private void GetReferences()
        {
            questInspectorContent = this.Q<VisualElement>("quest-inspector__content");

            //idTextField = this.Q<TextField>("quest-id__text-field");
            nameField = this.Q<TextField>("quest-name__text-field");
            descriptionField = this.Q<TextField>("quest-description__text-field");
            failableToggle = this.Q<Toggle>("quest-failable__toggle");
            nextQuestDropdown = this.Q<QuestDropdown>();
            onStartCommandListView = this.Q<CommandListView>("on-start-quest"); 
            onCompleteCommandListView = this.Q<CommandListView>("on-complete-quest");

            openQuestGraphButton = this.Q<Button>("open-quest-graph__button");
        }

        private void RegisterCallbacks()
        {
            descriptionField.RegisterCallback<FocusOutEvent>(e =>
            {
                descriptionField.value = descriptionField.value.Trim();
                quest.description = descriptionField.value;
            });

            failableToggle.RegisterValueChangedCallback(e => { quest.Failable = e.newValue; });

            openQuestGraphButton.clicked += OnOpenQuestGraphClicked;

            RegisterCallback<DetachFromPanelEvent>(e => UnregisterCallbacks());
        }

        private void UnregisterCallbacks()
        {
            if (this.quest != null)
            {
                EditorUtility.SetDirty(this.quest);
                AssetDatabase.SaveAssets();
            }
        }

        public void BindQuest(QuestSO quest)
        {
            if (this.quest != null)
            {
                EditorUtility.SetDirty(this.quest);
                AssetDatabase.SaveAssets();
            }

            this.Unbind();

            UnbindName();

            this.quest = quest;

            this.Bind(new SerializedObject(quest));

            BindName();
            BindOnStartCommandList();
            BindOnCompleteCommandList();

            nextQuestDropdown.SetQuest(quest.NextQuest);
        }

        private void BindName()
        {
            nameField.Bind(new SerializedObject(quest));
            nameField.RegisterValueChangedCallback(e =>  QuestValidator.ValidateAndSetName(e.previousValue, e.newValue, quest));
        }

        private void UnbindName()
        {
            nameField.Unbind();
            nameField.UnregisterValueChangedCallback(e => QuestValidator.ValidateAndSetName(e.previousValue, e.newValue, quest));
        }

        private void BindOnStartCommandList()
        {
            onStartCommandListView.PopulateView(quest.onStart);
            onCompleteCommandListView.SaveAsSubAssetOf(QuestGraphEditorWindow.questGraph);
        }
        
        private void BindOnCompleteCommandList()
        {
            onCompleteCommandListView.PopulateView(quest.onComplete);
            onCompleteCommandListView.SaveAsSubAssetOf(QuestGraphEditorWindow.questGraph);
        }

        public void HideContent()
        {
            questInspectorContent.visible = false;
        }

        public void ShowContent()
        {
            questInspectorContent.visible = true;
        }

        public void HideOpenGraphButton()
        {
            openQuestGraphButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        private void SetNextQuest(QuestSO nextQuest)
        {
            quest.NextQuest = nextQuest;
        }

        private void OnOpenQuestGraphClicked()
        {
            QuestGraphSO questGraph = QuestManager.instance.GetQuestGraph(quest);

            var questDesignerEditorWindow =
                (QuestDesignerEditorWindow)EditorWindow.GetWindow(typeof(QuestDesignerEditorWindow));
            questDesignerEditorWindow.OpenQuestGraph(questGraph);
        }
    }
}