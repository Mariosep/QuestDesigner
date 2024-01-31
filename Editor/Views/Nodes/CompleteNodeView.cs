using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class CompleteNodeView : NodeView
    {
        public CompleteNodeView(CompleteNodeSO node) : base(node, QuestGraphEditorWindow.RelativePath + "/UXML/Nodes/CompleteNodeView.uxml")
        {
            capabilities &= ~Capabilities.Deletable;

            style.minWidth = new StyleLength(80);
            style.maxHeight = new StyleLength(80);
            
            Label label = this.Q<Label>("type");
            label.name = "terminal-label";
        }

        protected override void CreateInputPorts()
        {
            inputPortsList = new List<Port>();

            var inputPort =
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Complete";

            inputPortsList.Add(inputPort);
            inputContainer.Add(inputPort);
        }
        
        protected override void CreateOutputPorts(){}
    }
}