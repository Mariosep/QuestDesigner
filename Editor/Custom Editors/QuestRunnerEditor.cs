using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    [CustomEditor(typeof(QuestRunner))]

    public class QuestRunnerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            Button openQuestDesignerButton = new Button(OpenQuestDesigner);
            openQuestDesignerButton.text = "Open Quest Designer";

            // TODO: Extract to a CustomButtonView
            openQuestDesignerButton.style.marginTop = 20;
            openQuestDesignerButton.style.marginBottom = 10;
            openQuestDesignerButton.style.paddingBottom = 5;
            openQuestDesignerButton.style.paddingTop = 5;
            openQuestDesignerButton.style.maxWidth = 220;
            openQuestDesignerButton.style.alignSelf = new StyleEnum<Align>(Align.Center);

            root.Add(openQuestDesignerButton);
            return root;
        }

        private void OpenQuestDesigner()
        {
            var questDesignerEditorWindow = QuestDesignerEditorWindow.OpenWindow();
            questDesignerEditorWindow.ShowRuntimeTab();
        }
    }

}