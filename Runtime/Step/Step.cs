using System;
using System.Collections.Generic;

namespace QuestDesigner
{
    [Serializable]

    public abstract class Step
    {
        public StepSO stepData;
        public Quest quest;

        public StepState stepState = StepState.Pending;

        public bool CompleteQuest => stepData.completeQuest;
        public bool FailQuest => stepData.failQuest;

        public List<StepSO> NextSteps => stepData.nextSteps;
        public bool HasNextTasks => NextSteps.Count > 0;

        protected QuestChannel questChannel;

        public Step(StepSO stepData, Quest quest, QuestChannel questChannel)
        {
            this.stepData = stepData;
            this.quest = quest;

            this.questChannel = questChannel;
        }

        public StepState Update()
        {
            if (stepState == StepState.Pending)
            {
                OnStart();

                stepState = StepState.InProgress;
            }

            stepState = OnUpdate();

            if (stepState == StepState.Completed)
                OnComplete();

            return stepState;
        }

        protected abstract void OnStart();
        protected abstract StepState OnUpdate();
        protected abstract void OnComplete();
    }

    public enum StepState
    {
        Pending,
        InProgress,
        Completed,
        Failed
    }
}