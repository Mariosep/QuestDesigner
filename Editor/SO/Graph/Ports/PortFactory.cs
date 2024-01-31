using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public static class PortFactory
    {
        public static PortSO Create(string portName, NodeSO originNode, List<NodeSO> targetNodes = null)
        {
            var port = ScriptableObject.CreateInstance<PortSO>();
            port.id = GUID.Generate().ToString();
            port.portName = portName;
            port.originNode = originNode;
            if (targetNodes != null)
                port.targetNodes = targetNodes;
            else
                port.targetNodes = new List<NodeSO>();
            return port;
        }
    }
}