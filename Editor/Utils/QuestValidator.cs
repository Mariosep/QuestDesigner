using System.Text.RegularExpressions;

namespace QuestDesigner.Editor
{
    public static class QuestValidator
    {
        public static string GetValidName(QuestDataBase questDataBase, string previousName, string newName, QuestSO quest,
            bool isNewQuest = false)
        {
            newName = newName.Trim();

            if (!isNewQuest && newName == previousName)
                return previousName;

            if (newName == "")
                return previousName;

            if (Regex.IsMatch(newName, "_[0-9]$") && questDataBase.ContainsQuestWithName(newName, quest))
                newName = Regex.Replace(newName, "_[0-9]$", "");

            string newNameWithNumber = newName;

            int count = 0;
            while (questDataBase.ContainsQuestWithName(newNameWithNumber, quest))
            {
                count++;
                newNameWithNumber = $"{newName}_{count}";
            }

            return newNameWithNumber;
        }
        
        public static void ValidateAndSetName(string previousName, string newName, QuestSO quest)
        {
            var questDataBase = QuestManager.instance.QuestDataBase;
            string validName = GetValidName(questDataBase, previousName, newName, quest);
            quest.QuestName = validName;
        }
    }

}