using System.Linq;

namespace QuestDesigner.Editor
{
    public class StartNodeSO : NodeSO
    {
        protected override void CreateOutputPorts()
        {
            var outputPort = PortFactory.Create("Start", this);
            outputPorts.Add(outputPort);
        }

        public override void AddOutputNode(string portId, NodeSO outputNode)
        {
            var port = outputPorts.First(port => port.id == portId);
            port.AddTargetNode(outputNode);
        }

        public override void RemoveOutputNode(string portId, NodeSO outputNode)
        {
            var port = outputPorts.First(port => port.id == portId);
            port.RemoveTargetNode(outputNode);
        }

        public override void RemoveAllOutputNodes()
        {
        }
    }
}