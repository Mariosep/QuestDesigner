using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestDesigner
{
    [Serializable]

    public class Quest
    {
        public Action<Quest> onQuestStarted;
        public Action<Quest> onQuestCompleted;
        public Action<Quest> onQuestFailed;
        public Action<Step> onStepStarted;
        public Action<Step> onStepCompleted;

        public QuestSO questData;

        public StepState stepState = StepState.Pending;

        private List<Step> currentSteps = new List<Step>();
        private List<Step> completedSteps = new List<Step>();

        /*private List<Task> currentTasks = new List<Task>();
    private List<Task> completedTasks = new List<Task>();*/

        private QuestChannel questChannel;

        public List<Step> CurrentTasks => currentSteps;
        public List<Step> CompletedTasks => completedSteps;

        /*public List<Task> CurrentTasks => currentTasks.Where(e => !e.taskData.isHidden).ToList();
    public List<Task> CompletedTasks => completedTasks.Where(e => !e.taskData.isHidden).ToList();*/

        public QuestSO NextQuest => questData.NextQuest;

        public Quest(QuestSO questData)
        {
            this.questData = questData;

            questChannel = ServiceLocator.Get<QuestChannel>();
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
            {
                OnComplete();
            }
            else if (stepState == StepState.Failed)
            {
                OnFail();
            }

            return stepState;
        }

        private void OnStart()
        {
            currentSteps = new List<Step>();

            Step initialStep = StepFactory.Create(questData.initialStep, this, questChannel);
            currentSteps.Add(initialStep);

            /*currentTasks = new List<Task>();
        currentTasks.Add(new Task(questData.initialTask, this, questChannel));*/

            onQuestStarted?.Invoke(this);
            questData.onComplete.Execute();
            questChannel.onQuestStarted?.Invoke(this);
            Debug.Log("On Quest Started");
        }

        // TODO: Refactor splitting in different methods
        private StepState OnUpdate()
        {
            List<Step> newCompletedSteps = new List<Step>();

            foreach (Step step in currentSteps)
            {
                if (step.stepState != StepState.Completed)
                {
                    step.Update();

                    if (step.stepState == StepState.Completed)
                    {
                        newCompletedSteps.Add(step);
                    }
                }
            }

            AddCompletedStepsToList(newCompletedSteps);

            return stepState;
        }

        private void AddCompletedStepsToList(List<Step> newCompletedSteps)
        {
            foreach (Step completedStep in newCompletedSteps)
            {
                currentSteps.Remove(completedStep);
                completedSteps.Add(completedStep);

                onStepCompleted?.Invoke(completedStep);

                if (completedStep.HasNextTasks)
                {
                    foreach (StepSO nextStepData in completedStep.NextSteps)
                    {
                        Step nextStep = StepFactory.Create(nextStepData, this, questChannel);
                        currentSteps.Add(nextStep);
                    }
                }
                else
                {
                    if (completedStep.CompleteQuest)
                        stepState = StepState.Completed;
                    else if (completedStep.FailQuest)
                        stepState = StepState.Failed;
                }
            }
        }

        protected void OnComplete()
        {
            Debug.Log("On Quest Completed");
            questData.onComplete.Execute();
            onQuestCompleted?.Invoke(this);
        }

        protected void OnFail()
        {
            Debug.Log("On Quest Failed");
            onQuestFailed?.Invoke(this);
            questChannel.onQuestFailed?.Invoke(this);
        }
    }
}