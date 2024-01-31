using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace QuestDesigner
{
    [CreateAssetMenu(menuName = "QuestDesigner/QuestDataBase", fileName = "QuestDataBase")]

    public class QuestDataBase : ScriptableObject
    {
        public List<QuestSO> questsList = new List<QuestSO>();

        [SerializedDictionary("ID", "Quest")]
        public SerializedDictionary<string, QuestSO> questsDic = new SerializedDictionary<string, QuestSO>();

        public void AddQuest(QuestSO quest)
        {
            questsList.Add(quest);
            questsDic.Add(quest.id, quest);
        }

        public void RemoveQuest(string id)
        {
            if (questsDic.TryGetValue(id, out QuestSO quest))
            {
                questsList.Remove(quest);
                questsDic.Remove(id);
            }
            else
                throw new Exception("Can not remove quest because is not registered.");
        }

        public bool TryGetElement(string id, out QuestSO quest)
        {
            if (questsDic.TryGetValue(id, out quest))
                return true;
            else
                return false;
        }

        public bool ContainsQuestWithName(string questName, QuestSO questToIgnore = null)
        {
            var questsFound = questsList.Where(e => e.QuestName == questName).ToList();

            if (questToIgnore != null && questsFound.Contains(questToIgnore))
                questsFound.Remove(questToIgnore);

            return questsFound.Count > 0;
        }

        public List<KeyValuePair<QuestSO, string>> GetPairs()
        {
            var pairs = new List<KeyValuePair<QuestSO, string>>();

            foreach (QuestSO quest in questsList)
            {
                pairs.Add(new KeyValuePair<QuestSO, string>(quest, $"{quest.QuestName}"));
            }

            return pairs;
        }
    }
}