using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestsListView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestsListView, UxmlTraits>{}
        
        private readonly string uxmlPath = "UXML/QuestsListView.uxml";

        public Action<QuestSO> onQuestSelected;
        public Action<QuestSO> onQuestChosen;

        private MultiColumnListView _listView;
        private QuestDataBase _questDataBase;
        private List<QuestSO> _quests => _questDataBase.questsList;

        public QuestSO[] questsSelected => _listView.selectedItems.Cast<QuestSO>().ToArray();

        public QuestsListView()
        {
            string path = Path.Combine(QuestDesignerEditorWindow.RelativePath, uxmlPath);
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            uxml.CloneTree(this);

            _listView = this.Q<MultiColumnListView>();
            _listView.selectionChanged += OnQuestSelected;
            _listView.itemsChosen += OnQuestChosen;

            Setup();
        }

        private void Setup()
        {
            _listView.columns["name"].makeCell = MakeNameCell;
            _listView.columns["description"].makeCell = MakeDescriptionCell;
            _listView.columns["enableOnStart"].makeCell = MakeEnableOnStartCell;
            _listView.columns["nextQuest"].makeCell = MakeNextQuestCell;

            _listView.columns["name"].bindCell = (element, i) => BindName(element, new SerializedObject(_quests[i]));
            _listView.columns["description"].bindCell =
                (element, i) => BindDescription(element, new SerializedObject(_quests[i]));
            _listView.columns["enableOnStart"].bindCell =
                (element, i) => BindEnableOnStart(element, new SerializedObject(_quests[i]));
            _listView.columns["nextQuest"].bindCell = (element, i) => BindNextQuest(element, _quests[i]);
        }

        public void Populate(QuestDataBase questDataBase)
        {
            _questDataBase = questDataBase;
            _listView.itemsSource = _quests;
            _listView.RefreshItems();

            if (_quests.Count > 0)
                _listView.SetSelection(0);
        }

        #region Modify list

        public void Add(QuestGraphSO questGraph)
        {
            string validName = QuestValidator.GetValidName(_questDataBase, "", questGraph.quest.QuestName, questGraph.quest, true);
            questGraph.quest.QuestName = validName;

            QuestManager.instance.QuestGraphDataBase.AddQuestGraph(questGraph);

            _listView.RefreshItems();

            _listView.SetSelection(_quests.Count - 1);
        }

        public void Remove(params QuestGraphSO[] questGraphsToDelete)
        {
            foreach (QuestGraphSO questGraph in questGraphsToDelete)
                QuestManager.instance.QuestGraphDataBase.RemoveQuestGraph(questGraph.id);

            _listView.RefreshItems();
        }

        #endregion

        #region Make

        private VisualElement MakeCell()
        {
            var cell = new VisualElement();
            cell.AddToClassList("centered-vertical");
            cell.style.paddingTop = 5f;
            cell.style.paddingBottom = 5f;

            return cell;
        }

        private VisualElement MakeNameCell()
        {
            var cell = MakeCell();

            var nameField = new Label("");
            nameField.name = "name-field";
            nameField.bindingPath = "questName";

            cell.Add(nameField);

            return cell;
        }

        private VisualElement MakeEnableOnStartCell()
        {
            var cell = MakeCell();
            cell.AddToClassList("centered-horizontal");

            var enableOnStartToggle = new Toggle("");
            enableOnStartToggle.name = "enableOnStartToggle-toggle";
            enableOnStartToggle.bindingPath = "enableOnStart";

            cell.Add(enableOnStartToggle);

            return cell;
        }

        private VisualElement MakeDescriptionCell()
        {
            var cell = MakeCell();

            var descriptionField = new Label("");
            descriptionField.name = "description-field";
            descriptionField.bindingPath = "description";

            cell.Add(descriptionField);

            return cell;
        }

        private VisualElement MakeNextQuestCell()
        {
            var cell = MakeCell();

            var nextQuestField = new Label("");
            nextQuestField.name = "nextQuest-field";

            cell.Add(nextQuestField);

            return cell;
        }

        #endregion

        #region Bind

        private void BindName(VisualElement cell, SerializedObject serializedObject)
        {
            Label nameLabel = cell.Q<Label>();
            nameLabel.Bind(serializedObject);
        }

        private void BindDescription(VisualElement cell, SerializedObject serializedObject)
        {
            Label descriptionLabel = cell.Q<Label>();
            descriptionLabel.Bind(serializedObject);
        }

        private void BindEnableOnStart(VisualElement cell, SerializedObject serializedObject)
        {
            Toggle enableOnStartToggle = cell.Q<Toggle>();
            enableOnStartToggle.Bind(serializedObject);
        }

        private void BindNextQuest(VisualElement cell, QuestSO quest)
        {
            Label nextQuestLabel = cell.Q<Label>();

            nextQuestLabel.text = "";

            if (quest.NextQuest != null)
                nextQuestLabel.text = quest.NextQuest.QuestName;

            quest.onNextQuestChanged += _ =>
            {
                if (quest.NextQuest != null)
                    nextQuestLabel.text = quest.NextQuest.QuestName;
                else
                    nextQuestLabel.text = "";
            };
        }

        #endregion

        private void OnQuestSelected(IEnumerable<object> itemsSelected)
        {
            int questIndex = _listView.selectedIndex;

            if (questIndex >= 0 && questIndex < _quests.Count)
                onQuestSelected?.Invoke(_quests[questIndex]);
        }

        private void OnQuestChosen(IEnumerable<object> itemChosen)
        {
            QuestSO quest = (QuestSO)itemChosen.ElementAt(0);

            onQuestChosen?.Invoke(quest);
        }
    }
}