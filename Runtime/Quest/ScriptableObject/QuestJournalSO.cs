using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestDesigner
{
    [CreateAssetMenu(menuName = "Quest/QuestJournal", fileName = "QuestJournal")]

    public class QuestJournalSO : ScriptableObject
    {
        public static Action<QuestJournalSO> onQuestJournalLoaded;
        
        public QuestDataBase questDataBase;

        public List<Quest> currentQuests = new List<Quest>();
        public List<Quest> completedQuests = new List<Quest>();

        private QuestChannel questChannel;

        public void Init()
        {
            currentQuests = new List<Quest>();
            completedQuests = new List<Quest>();

            questChannel = ServiceLocator.Get<QuestChannel>();

            if (questDataBase == null)
                return;

            foreach (QuestSO questData in questDataBase.questsList)
            {
                if (questData.enableOnStart)
                    AddQuest(questData);
            }
            
            onQuestJournalLoaded?.Invoke(this);
        }

        public void AddQuest(QuestSO questData)
        {
            Quest quest = new Quest(questData);

            currentQuests.Add(quest);
        }

        public void Update()
        {
            List<Quest> newCompletedQuests = new List<Quest>();

            foreach (Quest quest in currentQuests)
            {
                if (quest.stepState != StepState.Completed && quest.stepState != StepState.Failed)
                {
                    quest.Update();
                }
                else
                {
                    newCompletedQuests.Add(quest);
                }
            }

            AddCompletedQuestsToList(newCompletedQuests);
        }

        private void AddCompletedQuestsToList(List<Quest> newCompletedQuests)
        {
            foreach (Quest completedQuest in newCompletedQuests)
            {
                currentQuests.Remove(completedQuest);
                completedQuests.Add(completedQuest);

                questChannel.onQuestCompleted?.Invoke(completedQuest);

                if (completedQuest.NextQuest != null)
                    AddQuest(completedQuest.NextQuest);
            }
        }

        public Quest GetQuest(string id)
        {
            foreach (Quest quest in currentQuests)
            {
                if (quest.questData.id == id)
                    return quest;
            }

            return null;
        }
    }
}