using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestsRuntimeListView : VisualElement
    {
        private readonly string uxmlPath = "UXML/QuestsRuntimeListView.uxml";

        public Action<QuestSO> onQuestSelected;
        public Action<QuestSO> onQuestChosen;

        private MultiColumnListView _listView;
        private QuestJournalSO _questJournal;

        private List<Quest> _currentQuests => _questJournal.currentQuests;

        public QuestsRuntimeListView()
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
            _listView.columns["nextQuest"].makeCell = MakeNextQuestCell;

            _listView.columns["name"].bindCell = (element, i) =>
                BindName(element, new SerializedObject(_currentQuests[i].questData));
            _listView.columns["description"].bindCell = (element, i) =>
                BindDescription(element, new SerializedObject(_currentQuests[i].questData));
            _listView.columns["nextQuest"].bindCell =
                (element, i) => BindNextQuest(element, _currentQuests[i].questData);
        }

        public void Populate(QuestJournalSO questJournal)
        {
            _questJournal = questJournal;
            _listView.itemsSource = _currentQuests;
            _listView.RefreshItems();

            if (_currentQuests.Count > 0)
                _listView.SetSelection(0);
        }

        public void UpdateQuestList()
        {
            _listView.RefreshItems();
        }

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

        private void BindNextQuest(VisualElement cell, QuestSO quest)
        {
            Label nextQuestLabel = cell.Q<Label>();

            nextQuestLabel.text = "";

            if (quest.NextQuest != null)
                nextQuestLabel.text = quest.NextQuest.QuestName;
        }

        #endregion

        private void OnQuestSelected(IEnumerable<object> itemsSelected)
        {
            int questIndex = _listView.selectedIndex;

            if (questIndex >= 0 && questIndex < _currentQuests.Count)
                onQuestSelected?.Invoke(_currentQuests[questIndex].questData);
        }

        private void OnQuestChosen(IEnumerable<object> itemChosen)
        {
            Quest quest = (Quest)itemChosen.ElementAt(0);

            onQuestChosen?.Invoke(quest.questData);
        }
    }
}