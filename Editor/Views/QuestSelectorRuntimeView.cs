using System;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestSelectorRuntimeView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestSelectorRuntimeView, UxmlTraits>{}
        
        private readonly string uxmlPath = "UXML/QuestSelectorRuntime.uxml";

        public Action<QuestSO> onQuestSelected;
        public Action<QuestSO> onQuestChosen;

        private QuestJournalSO _questJournal;

        private QuestsRuntimeListView _questsRuntimeListView;

        private VisualElement _questsContainer;
        private CustomHelpBox editorModeHelpBox;

        private QuestChannel _questChannel;

        public QuestSelectorRuntimeView()
        {
            string path = Path.Combine(QuestGraphEditorWindow.RelativePath, uxmlPath);
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            uxml.CloneTree(this);

            _questsContainer = this.Q<VisualElement>("quest__container");
            editorModeHelpBox = this.Q<CustomHelpBox>("editor-mode__help-box");
        }

        public void PopulateView(QuestJournalSO questJournal)
        {
            _questJournal = questJournal;

            editorModeHelpBox.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

            if (_questsRuntimeListView != null)
                _questsContainer.Remove(_questsRuntimeListView);

            _questsRuntimeListView = new QuestsRuntimeListView();
            _questsRuntimeListView.onQuestSelected += OnQuestSelected;
            _questsRuntimeListView.onQuestChosen += OnQuestChosen;
            _questsRuntimeListView.Populate(_questJournal);

            _questsContainer.Add(_questsRuntimeListView);

            _questChannel = ServiceLocator.Get<QuestChannel>();
            _questChannel.onQuestCompleted += UpdateQuestList;


        }

        private void UpdateQuestList(Quest questCompleted)
        {
            _questsRuntimeListView.UpdateQuestList();
        }

        public void ShowEditModeHelpBox()
        {
            if (_questsRuntimeListView != null)
            {
                _questsContainer.Remove(_questsRuntimeListView);
                _questsRuntimeListView = null;

                _questChannel.onQuestCompleted -= UpdateQuestList;
            }

            editorModeHelpBox.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        private void OnQuestSelected(QuestSO questSelected)
        {
            onQuestSelected?.Invoke(questSelected);
        }

        private void OnQuestChosen(QuestSO questChosen)
        {
            onQuestChosen?.Invoke(questChosen);
        }
    }
}