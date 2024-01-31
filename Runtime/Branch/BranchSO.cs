using Blackboard.Requirement;
using UnityEngine;

namespace QuestDesigner
{
    public class BranchSO : StepSO
    {
        public RequirementsSO requirements;

        public StepSO nextStepIfTrue;
        public StepSO nextStepIfFalse;

        public override void Init(string id)
        {
            this.id = id;
            name = $"branch-{id}";
            requirements = ScriptableObject.CreateInstance<RequirementsSO>();
            requirements.Init(id);
        }

        public void SetNextStepIfTrue(StepSO step) => nextStepIfTrue = step;
        public void SetNextStepIfFalse(StepSO step) => nextStepIfFalse = step;
        public void RemoveNextStepIfTrue(StepSO step) => nextStepIfTrue = null;
        public void RemoveNextStepIfFalse(StepSO step) => nextStepIfFalse = null;
    }
}