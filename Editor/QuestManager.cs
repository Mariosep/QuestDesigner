using System.Collections.Generic;
using UnityEditor;
using UnityEditorForks;

namespace QuestDesigner.Editor
{
    public class QuestManager : ScriptableSingleton<QuestManager>
    {
        private QuestDesignerSO questDesigner;

        public QuestDataBase QuestDataBase
        {
            get
            {
                if (questDesigner == null)
                    FindQuestDesignerDataBase();

                return questDesigner.questDataBase;
            }
        }

        public QuestGraphDataBase QuestGraphDataBase
        {
            get
            {
                if (questDesigner == null)
                    FindQuestDesignerDataBase();

                return questDesigner.questGraphDataBase;
            }
        }

        private void FindQuestDesignerDataBase()
        {
            List<QuestDesignerSO> assetsList = typeof(QuestDesignerSO).FindAssetsByType<QuestDesignerSO>();
            if (assetsList.Count > 0)
                questDesigner = assetsList[0];
            else
            {
                questDesigner = QuestDesignerFactory.CreateQuestDesigner("QuestDesigner");
            }
        }

        public string GetQuestPath(string id)
        {
            if (QuestDataBase.TryGetElement(id, out QuestSO quest))
                return quest.QuestName;

            return "";
        }

        public QuestGraphSO GetQuestGraph(QuestSO quest)
        {
            return QuestGraphDataBase.GetElement(quest.id);
        }

        public QuestGraphSO GetQuestGraph(string id)
        {
            return QuestGraphDataBase.GetElement(id);
        }
    }
}