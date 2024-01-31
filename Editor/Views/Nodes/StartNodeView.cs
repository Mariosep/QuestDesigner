using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class StartNodeView : NodeView
    {
        public StartNodeView(StartNodeSO node) : base(node, QuestGraphEditorWindow.RelativePath + "/UXML/Nodes/StartNodeView.uxml")
        {
            capabilities = capabilities & ~Capabilities.Deletable;

            style.minWidth = new StyleLength(80);
            style.maxHeight = new StyleLength(80);

            Label label = this.Q<Label>("type");
            label.name = "terminal-label";
        }

        protected override void CreateInputPorts(){}
    }
}