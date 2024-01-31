using System;
using Blackboard.Requirement;
using UnityEngine;

namespace QuestDesigner
{
    [Serializable]

    public class Task : Step
    {
        public TaskSO taskData;
        public Requirements requirements;

        public Task(TaskSO taskData, Quest quest, QuestChannel questChannel) : base(taskData, quest, questChannel)
        {
            this.taskData = taskData;
            requirements = new Requirements(taskData.requirements);
        }

        protected override void OnStart()
        {
            Debug.Log("On Task Started");
            
            taskData.onStart.Execute();
            quest.onStepStarted?.Invoke(this);
            questChannel.onTaskStarted?.Invoke(this);
        }

        protected override StepState OnUpdate()
        {
            if (AreRequirementsFulfilled())
                return StepState.Completed;
            else
                return StepState.InProgress;
        }

        protected override void OnComplete()
        {
            Debug.Log("On Task Completed");
            taskData.onComplete.Execute();
            quest.onStepCompleted?.Invoke(this);
            questChannel.onTaskCompleted?.Invoke(this);
        }

        public bool AreRequirementsFulfilled()
        {
            return requirements.AreFulfilled;
        }
    }
}