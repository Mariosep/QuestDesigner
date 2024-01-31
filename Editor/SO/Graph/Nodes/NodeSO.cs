using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public abstract class NodeSO : ScriptableObject
    {
        [HideInInspector] public string id;
        [HideInInspector] public Rect position;

        public StepSO step;

        // TODO: Create input ports
        public List<PortSO> outputPorts = new List<PortSO>();

        public virtual void Init(Vector2 position)
        {
            this.id = GUID.Generate().ToString();
            this.position = new Rect(position, Vector2.zero);

            CreateOutputPorts();
        }

        protected virtual void CreateOutputPorts()
        {
            var outputPort = PortFactory.Create("Next", this);
            outputPorts.Add(outputPort);
        }

        public virtual void AddOutputNode(string portId, NodeSO outputNode)
        {
            var port = outputPorts.First(port => port.id == portId);
            port.AddTargetNode(outputNode);

            if (outputNode is not CompleteNodeSO)
                step.nextSteps.Add(outputNode.step);
        }

        public virtual void RemoveOutputNode(string portId, NodeSO outputNode)
        {
            var port = outputPorts.First(port => port.id == portId);
            port.RemoveTargetNode(outputNode);

            if (outputNode is not CompleteNodeSO)
                step.nextSteps.Remove(outputNode.step);
        }

        public virtual void RemoveAllOutputNodes()
        {
            outputPorts.ForEach(p => p.RemoveAllTargetNodes());

            step.nextSteps.Clear();
        }

        public virtual NodeSO Clone()
        {
            string newId = GUID.Generate().ToString();

            NodeSO clonedNode = Instantiate(this);
            clonedNode.id = newId;

            return clonedNode;
        }

        public virtual void SaveAs(QuestGraphSO questGraph)
        {
            AssetDatabase.AddObjectToAsset(this, questGraph);

            foreach (PortSO outputPort in outputPorts)
                AssetDatabase.AddObjectToAsset(outputPort, questGraph);

            AssetDatabase.SaveAssets();
        }

        protected virtual void OnDestroy()
        {
            EditorUtility.SetDirty(this);

            foreach (PortSO outputPort in outputPorts)
                AssetDatabase.RemoveObjectFromAsset(outputPort);

            AssetDatabase.SaveAssets();
        }
    }

    public enum NodeType
    {
        Start,
        Complete,
        Fail,
        Task,
        Branch
    }
}