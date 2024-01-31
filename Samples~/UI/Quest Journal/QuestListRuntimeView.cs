using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestDesigner
{
    public class QuestListRuntimeView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestListRuntimeView, UxmlTraits>
        {
        }

        public Action<Quest> onQuestSelected;

        private ListView currentQuestListView;
        private ListView completedQuestListView;

        private VisualTreeAsset questItemAsset;

        private QuestJournalSO questJournal;
        private QuestChannel questChannel;

        public QuestListRuntimeView()
        {
            var uxml = Resources.Load<VisualTreeAsset>("UXML/QuestJournal/QuestListView");
            uxml.CloneTree(this);

            questItemAsset = Resources.Load<VisualTreeAsset>("UXML/QuestJournal/QuestListViewItem");

            currentQuestListView = this.Q<ListView>("current-quests__list-view");
            completedQuestListView = this.Q<ListView>("completed-quests__list-view");

            Setup();
        }

        private void Setup()
        {
            currentQuestListView.makeItem = MakeItem;
            currentQuestListView.bindItem = (element, i) => BindItem(element, questJournal.currentQuests[i]);

            currentQuestListView.selectionChanged += OnCurrentQuestSelected;
            completedQuestListView.selectionChanged += OnCompletedQuestSelected;

            completedQuestListView.makeItem = MakeItem;
            completedQuestListView.bindItem = (element, i) => BindItem(element, questJournal.completedQuests[i]);

            RegisterCallback<DetachFromPanelEvent>(e => OnDestroy());
        }

        public void Populate(QuestJournalSO questJournal)
        {
            questChannel = ServiceLocator.Get<QuestChannel>();
            questChannel.onQuestStarted += OnQuestStarted;
            questChannel.onQuestCompleted += OnQuestCompleted;

            this.questJournal = questJournal;

            currentQuestListView.itemsSource = questJournal.currentQuests;
            OnCurrentQuestsListUpdated();

            completedQuestListView.itemsSource = questJournal.completedQuests;
            OnCompletedQuestsListUpdated();

            if (questJournal.currentQuests.Count > 0)
                currentQuestListView.SetSelection(0);
            else if (questJournal.completedQuests.Count > 0)
                completedQuestListView.SetSelection(0);
        }

        private void OnQuestStarted(Quest questStarted)
        {
            currentQuestListView.itemsSource = questJournal.currentQuests;
            currentQuestListView.RefreshItems();
            OnCurrentQuestsListUpdated();
        }

        private void OnQuestCompleted(Quest questCompleted)
        {
            currentQuestListView.itemsSource = questJournal.currentQuests;
            currentQuestListView.RefreshItems();
            OnCurrentQuestsListUpdated();

            completedQuestListView.itemsSource = questJournal.completedQuests;
            completedQuestListView.RefreshItems();
            OnCompletedQuestsListUpdated();
        }

        private void OnCurrentQuestSelected(IEnumerable<object> itemsSelected)
        {
            if (!itemsSelected.Any())
                return;

            Quest questSelected = itemsSelected.ToList()[0] as Quest;

            completedQuestListView.ClearSelection();
            onQuestSelected?.Invoke(questSelected);
        }

        private void OnCompletedQuestSelected(IEnumerable<object> itemsSelected)
        {
            if (!itemsSelected.Any())
                return;

            Quest questSelected = itemsSelected.ToList()[0] as Quest;

            currentQuestListView.ClearSelection();
            onQuestSelected?.Invoke(questSelected);
        }


        private VisualElement MakeItem()
        {
            var questItem = new VisualElement();
            questItemAsset.CloneTree(questItem);
            return questItem;
        }

        private void BindItem(VisualElement element, Quest quest)
        {
            QuestSO questData = quest.questData;

            var questLabel = element.Q<Label>();

            if (questData.QuestName != "")
                questLabel.text = questData.QuestName;
            else
                questLabel.text = "Unnamed Quest";

            if (quest.stepState == StepState.Completed)
                questLabel.text += " COMPLETED";
        }

        private void OnCurrentQuestsListUpdated()
        {
            if (currentQuestListView.itemsSource.Count == 0)
                currentQuestListView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            else
                currentQuestListView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        private void OnCompletedQuestsListUpdated()
        {
            if (completedQuestListView.itemsSource.Count == 0)
                completedQuestListView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            else
                completedQuestListView.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        private void OnDestroy()
        {
            if (questChannel != null)
            {
                questChannel.onQuestStarted -= OnQuestStarted;
                questChannel.onQuestCompleted -= OnQuestCompleted;
            }
        }
    }

}