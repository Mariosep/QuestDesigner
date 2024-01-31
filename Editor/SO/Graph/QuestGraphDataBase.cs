using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    [CreateAssetMenu(menuName = "QuestDesigner/QuestGraphDataBase", fileName = "QuestGraphDataBase")]

    public class QuestGraphDataBase : ScriptableObject
    {
        public List<QuestGraphSO> questGraphsList = new List<QuestGraphSO>();

        [SerializedDictionary("ID", "QuestGraph")]
        public SerializedDictionary<string, QuestGraphSO> questGraphsDic =
            new SerializedDictionary<string, QuestGraphSO>();

        private QuestDataBase questDataBase => QuestManager.instance.QuestDataBase;

        public void AddQuestGraph(QuestGraphSO questGraph)
        {
            questGraphsList.Add(questGraph);
            questGraphsDic.Add(questGraph.id, questGraph);

            questDataBase.AddQuest(questGraph.quest);

            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(questDataBase);
            AssetDatabase.SaveAssets();
        }

        public void RemoveQuestGraph(string id)
        {
            if (questGraphsDic.TryGetValue(id, out QuestGraphSO questGraph))
            {
                questGraphsList.Remove(questGraph);
                questGraphsDic.Remove(id);

                questDataBase.RemoveQuest(id);

                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
            else
                throw new Exception("Can not remove quest graph because is not registered.");
        }

        public bool TryGetElement(string id, out QuestGraphSO questGraph)
        {
            if (questGraphsDic.TryGetValue(id, out questGraph))
                return true;
            else
                return false;
        }

        public QuestGraphSO GetElement(string id)
        {
            return questGraphsDic[id];
        }
    }

}