using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public class NodeView : Node
    {
        public Action<NodeView> onNodeSelected;
        public Action<NodeView> onNodeUnselected;

        public NodeSO node;

        public List<Port> inputPortsList;
        public List<Port> outputPortsList;
       
        public NodeView(NodeSO node, string uxmlPath) : base(uxmlPath)
        {
            this.node = node;
            this.title = node.name.Split("-")[0];
            this.viewDataKey = node.id;

            SetPosition(node.position);

            CreateInputPorts();
            CreateOutputPorts();
        }

        protected virtual void CreateInputPorts()
        {
            inputPortsList = new List<Port>();

            var inputPort =
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Previous";

            inputPortsList.Add(inputPort);
            inputContainer.Add(inputPort);
        }

        protected virtual void CreateOutputPorts()
        {
            outputPortsList = new List<Port>();

            foreach (PortSO outputPortData in node.outputPorts)
            {
                var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                    typeof(bool));
                outputPort.portName = outputPortData.portName;
                outputPort.viewDataKey = outputPortData.id;

                outputPortsList.Add(outputPort);
                outputContainer.Add(outputPort);
            }
        }

        public Port GetOutputPortById(string portId)
        {
            return outputPortsList.First(port => port.viewDataKey == portId);
        }

        public Port GetOutputPortByName(string portName)
        {
            return outputPortsList.First(port => port.portName == portName);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(node, "Set Node Position");

            node.position = newPos;

            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();

            onNodeSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();

            onNodeUnselected?.Invoke(this);
        }

        public void UpdateState(StepState state)
        {
            switch (state)
            {
                case StepState.Pending:
                    RemoveFromClassList("in-progress");
                    RemoveFromClassList("completed");
                    break;

                case StepState.InProgress:
                    AddToClassList("in-progress");
                    break;

                case StepState.Completed:
                    RemoveFromClassList("in-progress");
                    AddToClassList("completed");
                    break;
            }
        }

        public virtual void OnNodeEntered(Step step) {}
        public virtual void OnNodeExited() {}
    }
}