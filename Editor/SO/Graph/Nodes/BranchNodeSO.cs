using Blackboard.Requirement;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public class BranchNodeSO : NodeSO
    {
        public BranchSO branch;

        public PortSO TrueOutputPort => outputPorts[0];
        public PortSO FalseOutputPort => outputPorts[1];

        public override void Init(Vector2 position)
        {
            base.Init(position);

            name = $"BranchNode-{id}";
            branch = ScriptableObject.CreateInstance<BranchSO>();
            branch.Init(id);

            step = branch;
        }

        protected override void CreateOutputPorts()
        {
            var trueOutputPort = PortFactory.Create("True", this);
            outputPorts.Add(trueOutputPort);

            var falseOutputPort = PortFactory.Create("False", this);
            outputPorts.Add(falseOutputPort);
        }

        public override void AddOutputNode(string portId, NodeSO outputNode)
        {
            base.AddOutputNode(portId, outputNode);

            if (TrueOutputPort.id == portId)
                branch.SetNextStepIfTrue(outputNode.step);
            else if (FalseOutputPort.id == portId)
                branch.SetNextStepIfFalse(outputNode.step);
        }

        public override void RemoveOutputNode(string portId, NodeSO outputNode)
        {
            base.AddOutputNode(portId, outputNode);

            if (TrueOutputPort.id == portId)
                branch.RemoveNextStepIfTrue(outputNode.step);
            else if (FalseOutputPort.id == portId)
                branch.RemoveNextStepIfFalse(outputNode.step);
        }

        public override void SaveAs(QuestGraphSO questGraph)
        {
            base.SaveAs(questGraph);

            EditorUtility.SetDirty(this);

            AssetDatabase.AddObjectToAsset(branch, questGraph);
            AssetDatabase.AddObjectToAsset(branch.requirements, questGraph);

            foreach (ConditionSO cond in branch.requirements.conditions)
                AssetDatabase.AddObjectToAsset(cond, questGraph);

            AssetDatabase.SaveAssets();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);

                foreach (ConditionSO cond in branch.requirements.conditions)
                    AssetDatabase.RemoveObjectFromAsset(cond);

                AssetDatabase.RemoveObjectFromAsset(branch.requirements);
                AssetDatabase.RemoveObjectFromAsset(branch);
                AssetDatabase.SaveAssets();
            }
        }
    }

}