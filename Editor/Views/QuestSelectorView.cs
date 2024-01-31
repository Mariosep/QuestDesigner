using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blackboard.Utils.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestSelectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestSelectorView, UxmlTraits>{}
        
        private readonly string uxmlPath = "UXML/QuestSelectorEditor.uxml";

        public Action<QuestSO> onQuestSelected;
        public Action<QuestSO> onQuestChosen;

        private QuestDataBase _questDataBase;

        private QuestsListView _questListView;

        private VisualElement _questsContainer;
        private Button _addQuestButton;
        private Button _removeQuestButton;

        private List<QuestSO> _quests => _questDataBase.questsList;

        public QuestSelectorView()
        {
            string path = Path.Combine(QuestGraphEditorWindow.RelativePath, uxmlPath);
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            uxml.CloneTree(this);

            //_groupHeader = this.Q<GroupHeaderView>();
            _questsContainer = this.Q<VisualElement>("quest__container");
            _addQuestButton = this.Q<Button>("add-quest__button");
            _removeQuestButton = this.Q<Button>("remove-quest__button");

            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            _addQuestButton.clicked += OnAddQuestButtonClicked;
            _removeQuestButton.clicked += OnRemoveQuestButtonClicked;

            RegisterCallback<DetachFromPanelEvent>(_ => UnregisterCallbacks());
        }

        private void UnregisterCallbacks()
        {
            _addQuestButton.clicked -= OnAddQuestButtonClicked;
            _removeQuestButton.clicked -= OnRemoveQuestButtonClicked;
        }

        public void PopulateView(QuestDataBase questDataBase)
        {
            _questDataBase = questDataBase;

            if (_questListView != null)
                _questsContainer.Remove(_questListView);

            _questListView = new QuestsListView();
            _questListView.onQuestSelected += OnQuestSelected;
            _questListView.onQuestChosen += OnQuestChosen;
            _questListView.Populate(questDataBase);

            _questsContainer.Add(_questListView);
        }

        private void OnQuestSelected(QuestSO questSelected)
        {
            onQuestSelected?.Invoke(questSelected);
        }

        private void OnQuestChosen(QuestSO questChosen)
        {
            onQuestChosen?.Invoke(questChosen);
        }

        public void AddQuest()
        {
            QuestGraphSO newQuestGraph = QuestGraphFactory.CreateQuestGraph();

            ScriptableObjectUtility.SaveAsset(newQuestGraph,
                $"Assets/SO/Quest Designer/Graphs/{newQuestGraph.name}.asset");
            ScriptableObjectUtility.SaveSubAsset(newQuestGraph.quest, newQuestGraph);
            ScriptableObjectUtility.SaveSubAsset(newQuestGraph.quest.onStart, newQuestGraph);
            ScriptableObjectUtility.SaveSubAsset(newQuestGraph.quest.onComplete, newQuestGraph);

            _questListView.Add(newQuestGraph);
        }

        private void RemoveQuest(params QuestSO[] questsToDelete)
        {
            foreach (QuestSO quest in questsToDelete)
            {
                QuestGraphSO questGraph = QuestManager.instance.GetQuestGraph(quest);

                _questListView.Remove(questGraph);
                ScriptableObjectUtility.DeleteAsset(questGraph);
            }
        }

        private void OnAddQuestButtonClicked() => AddQuest();

        private void OnRemoveQuestButtonClicked()
        {
            QuestSO[] questsSelected = _questListView.questsSelected;

            if (questsSelected.Length == 0 && _quests.Count > 0)
                questsSelected = new[] { _quests.Last() };

            if (questsSelected.Length > 0)
            {
                ShowConfirmQuestDeletionPopUp(questsSelected);
            }
        }

        private void ShowConfirmQuestDeletionPopUp(params QuestSO[] quests)
        {
            bool deleteClicked = EditorUtility.DisplayDialog(
                "Delete quest selected?",
                "Are you sure you want to delete this quest",
                "Delete",
                "Cancel");

            if (deleteClicked)
            {
                RemoveQuest(quests);
            }
        }
    }
}