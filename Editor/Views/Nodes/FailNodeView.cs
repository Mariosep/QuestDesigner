using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class FailNodeView : NodeView
    {
        public FailNodeView(FailNodeSO node) : base(node, QuestGraphEditorWindow.RelativePath + "/UXML/Nodes/NodeView.uxml")
        {
            capabilities = capabilities & ~Capabilities.Deletable;

            style.minWidth = new StyleLength(80);
            style.maxHeight = new StyleLength(80);
        }

        protected override void CreateOutputPorts()
        {
        }
    }
}