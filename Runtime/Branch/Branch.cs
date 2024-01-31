using System;
using Blackboard.Requirement;
using QuestDesigner;

namespace QuestDesigner
{
    [Serializable]

    public class Branch : Step
    {
        public BranchSO branchData;
        public Requirements requirements;

        public Branch(BranchSO branchData, Quest quest, QuestChannel questChannel) : base(branchData, quest,
            questChannel)
        {
            this.branchData = branchData;
        }

        protected override void OnStart()
        {
            branchData.nextSteps.Clear();
            requirements = new Requirements(branchData.requirements);
        }

        protected override StepState OnUpdate()
        {
            if (AreRequirementsFulfilled())
                branchData.nextSteps.Add(branchData.nextStepIfTrue);
            else
                branchData.nextSteps.Add(branchData.nextStepIfFalse);

            return StepState.Completed;
        }

        protected override void OnComplete()
        {
            quest.onStepCompleted?.Invoke(this);
        }

        public bool AreRequirementsFulfilled()
        {
            return requirements.AreFulfilled;
        }
    }
}