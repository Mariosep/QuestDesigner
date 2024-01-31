using System;
using System.Collections.Generic;
using Blackboard.Commands;
using Blackboard.Requirement;

namespace QuestDesigner
{
    public class TaskSO : StepSO
    {
        public Action<string> onNameChanged;

        public string taskName;
        public string description;

        public RequirementsSO requirements;

        public bool isHidden;

        public CommandList onStart;
        public CommandList onComplete;

        public override void Init(string id)
        {
            this.id = id;
            name = $"task-{id}";
            taskName = "Task";
            requirements = CreateInstance<RequirementsSO>();
            requirements.Init(id);
            nextSteps = new List<StepSO>();
            onStart = CreateInstance<CommandList>();
            onComplete = CreateInstance<CommandList>();
            
            onStart.Init(id);
            onComplete.Init(id);
        }

        public void SetName(string newName)
        {
            taskName = newName;

            onNameChanged?.Invoke(newName);
        }

        public TaskSO Clone(string newId)
        {
            TaskSO clonedTask = Instantiate(this);
            clonedTask.id = newId;
            clonedTask.name = $"task-{newId}";

            clonedTask.requirements = requirements.Clone(newId);

            return clonedTask;
        }
    }
}