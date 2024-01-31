using System;
using System.Collections.Generic;
using System.IO;
using Blackboard.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestDropdown : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestDropdown, UxmlTraits>{}
        
        private const string UxmlPath = "UXML/ElementDropdown.uxml";

        public Action<QuestSO> onQuestSelected;

        public QuestSO questSelected;

        private bool addNoneOption = false;

        private Label nameLabel;
        private Button buttonPopup;

        public QuestDropdown()
        {
            string path = Path.Combine(BlackboardEditorWindow.RelativePath, UxmlPath);
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            uxml.CloneTree(this);

            style.flexGrow = 1;

            nameLabel = this.Q<Label>("name-label");

            buttonPopup = this.Q<Button>();
            buttonPopup.clicked += OpenSearchWindow;

            UpdateButtonText();
        }

        public void SetName(string questName)
        {
            nameLabel.text = questName;
            nameLabel.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        public void AddNoneOption()
        {
            addNoneOption = true;
        }

        private void OpenSearchWindow()
        {
            var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

            var questPairs = GetQuestPairs();

            if (addNoneOption)
                questPairs.Insert(0, new KeyValuePair<QuestSO, string>(null, "None"));

            var questSearchProvider = ScriptableObject.CreateInstance<QuestSearchProvider>();
            questSearchProvider.Init(questPairs, SetQuest);

            SearchWindow.Open(new SearchWindowContext(mousePos), questSearchProvider);
        }

        private List<KeyValuePair<QuestSO, string>> GetQuestPairs()
        {
            return QuestManager.instance.QuestDataBase.GetPairs();
        }

        public void BindQuest(QuestSO quest)
        {
            if (questSelected != null)
                questSelected.onNameChanged -= UpdateButtonText;

            questSelected = quest;

            if (quest != null)
                questSelected.onNameChanged += UpdateButtonText;
        }

        public void SetQuest(QuestSO newQuestSelected)
        {
            if (this.questSelected == newQuestSelected)
                return;

            BindQuest(newQuestSelected);

            UpdateButtonText();

            onQuestSelected?.Invoke(newQuestSelected);
        }

        private void UpdateButtonText()
        {
            string questPath = "";

            if (questSelected != null)
                questPath = QuestManager.instance.GetQuestPath(questSelected.id);

            buttonPopup.text = questPath;
        }
    }
}