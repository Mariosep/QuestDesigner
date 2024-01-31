using System;

namespace QuestDesigner
{
    public class QuestChannel
    {
        public Action<Quest> onQuestStarted;
        public Action<Quest> onQuestCompleted;
        public Action<Quest> onQuestFailed;
        public Action<Task> onTaskStarted;
        public Action<Task> onTaskCompleted;
    }
}