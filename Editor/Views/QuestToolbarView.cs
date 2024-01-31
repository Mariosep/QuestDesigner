using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestToolbarView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestToolbarView, UxmlTraits>{}
        
        private readonly string uxmlName = "QuestToolbar.uxml";

        private Toolbar _toolbar;

        private QuestSelectorView _questSelectorView;

        public QuestToolbarView()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(QuestGraphEditorWindow.RelativePath, uxmlName);
            uxml.CloneTree(this);

            _toolbar = this.Q<Toolbar>();
        }

        public void PopulateView(QuestSelectorView questSelectorView)
        {
            _questSelectorView = questSelectorView;

            ToolbarMenu assetsMenu = _toolbar.Q<ToolbarMenu>("toolbar-menu__assets");
            assetsMenu.menu.AppendAction("New quest", a => CreateNewQuest());
        }

        private void CreateNewQuest()
        {
            _questSelectorView.AddQuest();
        }
    }
}