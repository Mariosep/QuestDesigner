using Blackboard.Editor.Requirement;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class BranchNodeViewContainer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BranchNodeViewContainer, UxmlTraits>{}
        
        private readonly string uxmlName = "BranchNodeViewContainer.uxml";

        private RequirementsListView requirementsListView;

        public BranchNodeViewContainer()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(QuestGraphEditorWindow.RelativePath, uxmlName);
            uxml.CloneTree(this);

            requirementsListView = this.Q<RequirementsListView>();
        }

        public void PopulateView(BranchNodeSO node)
        {
            requirementsListView.SaveAsSubAssetOf(QuestGraphEditorWindow.questGraph);
            requirementsListView.PopulateView(node.branch.requirements);
        }
    }

}