using UnityEngine;

namespace QuestDesigner.Editor
{
    public static class QuestGraphFactory
    {
        public static QuestGraphSO CreateQuestGraph()
        {
            QuestGraphSO questGraph = ScriptableObject.CreateInstance<QuestGraphSO>();
            questGraph.Init();

            return questGraph;
        }
    }
}