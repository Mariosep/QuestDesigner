using UnityEngine;

namespace QuestDesigner.Editor
{
    public class QuestDesignerSO : ScriptableObject
    {
        public string id;

        public QuestDataBase questDataBase;
        public QuestGraphDataBase questGraphDataBase;
    }
}