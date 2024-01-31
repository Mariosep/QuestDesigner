using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestEditorInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestEditorInspectorView, UxmlTraits>{}
        
        private readonly string uxmlName = "QuestEditorInspector.uxml";

        private QuestInspectorView questInspectorView;

        public QuestEditorInspectorView()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(QuestGraphEditorWindow.RelativePath, uxmlName);
            uxml.CloneTree(this);

            style.flexGrow = 1;

            this.Clear();
        }

        public void BindQuest(QuestSO quest)
        {
            this.Clear();

            questInspectorView = new QuestInspectorView();
            questInspectorView.BindQuest(quest);

            this.Add(questInspectorView);
        }

        public void UnbindQuest()
        {
            this.Clear();

            questInspectorView = null;
        }
    }
}