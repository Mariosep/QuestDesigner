namespace QuestDesigner
{
    public static class StepFactory
    {
        public static Step Create(StepSO stepData, Quest quest, QuestChannel questChannel)
        {
            switch (stepData)
            {
                case TaskSO taskData:
                    return new Task(taskData, quest, questChannel);

                case BranchSO branchData:
                    return new Branch(branchData, quest, questChannel);
            }

            return null;
        }
    }
}