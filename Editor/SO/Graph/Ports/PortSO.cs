using System.Collections.Generic;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public class PortSO : ScriptableObject
    {
        public string id;
        public string portName;
        public NodeSO originNode;
        public List<NodeSO> targetNodes = new List<NodeSO>();

        public void AddTargetNode(NodeSO targetNode)
        {
            targetNodes.Add(targetNode);
        }

        public void RemoveTargetNode(NodeSO targetNode)
        {
            targetNodes.Remove(targetNode);
        }

        public void RemoveAllTargetNodes()
        {
            targetNodes.Clear();
        }
    }
}