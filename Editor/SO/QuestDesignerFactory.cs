using Blackboard.Utils.Editor;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public static class QuestDesignerFactory
    {
        public static QuestDesignerSO CreateQuestDesigner(string name, string id = null)
        {
            QuestDesignerSO questDesigner = ScriptableObject.CreateInstance<QuestDesignerSO>();
            questDesigner.name = name;

            if (id != null)
                questDesigner.id = id;
            else
                questDesigner.id = GUID.Generate().ToString();

            // Create QuestDataBase
            QuestDataBase questDataBase = ScriptableObject.CreateInstance<QuestDataBase>();
            questDesigner.questDataBase = questDataBase;

            // Create QuestDataBase
            QuestGraphDataBase questGraphDataBase = ScriptableObject.CreateInstance<QuestGraphDataBase>();
            questDesigner.questGraphDataBase = questGraphDataBase;

            // Save quest designer 
            ScriptableObjectUtility.SaveAsset(questDesigner, $"Assets/SO/Quest Designer/{questDesigner.name}.asset");

            // Save databases
            ScriptableObjectUtility.SaveSubAsset(questDesigner.questDataBase, questDesigner);
            ScriptableObjectUtility.SaveSubAsset(questDesigner.questGraphDataBase, questDesigner);

            return questDesigner;
        }
    }

}