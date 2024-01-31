using UnityEditor;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    [CustomPropertyDrawer(typeof(QuestSO))]

    public class QuestDrawer : PropertyDrawer
    {
        private SerializedProperty questProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            questProperty = property;

            var questDropdown = new QuestDropdown();
            questDropdown.SetName(property.name.Capitalize());

            if (questProperty.objectReferenceValue != null)
                questDropdown.SetQuest((QuestSO)questProperty.objectReferenceValue);

            questDropdown.onQuestSelected += SetQuest;

            questDropdown.style.marginTop = 5;
            questDropdown.style.marginBottom = 10;

            return questDropdown;
        }

        private void SetQuest(QuestSO questSelected)
        {
            questProperty.objectReferenceValue = questSelected;
            questProperty.serializedObject.ApplyModifiedProperties();
        }
    }

}