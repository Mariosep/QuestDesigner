using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestDesigner
{
    [RequireComponent(typeof(UIDocument))]

    public class QuestNotificationUIHandler : MonoBehaviour
    {
        public QuestNotificationSO questNotificationData;

        private UIDocument uiDocument;
        private QuestChannel questChannel;

        private Queue<QuestNotificationView> notificationQueue;
        private QuestNotificationView lastNotification;

        private void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            questChannel = ServiceLocator.Get<QuestChannel>();
            notificationQueue = new Queue<QuestNotificationView>();

            questChannel.onQuestStarted += OnQuestStarted;
            questChannel.onTaskCompleted += OnTaskCompleted;
            questChannel.onQuestCompleted += OnQuestCompleted;
            questChannel.onQuestFailed += OnQuestFailed;
        }

        private void Update()
        {
            if (notificationQueue.Count > 0)
            {
                if (lastNotification == null)
                {
                    ShowNextNotification();
                }
                else if (lastNotification.type == QuestNotificationView.NotificationType.QuestUpdate &&
                         notificationQueue.Peek().type == QuestNotificationView.NotificationType.QuestUpdate)
                {
                    ShowNextNotification();
                }
                else if (lastNotification.isCompleted)
                {
                    uiDocument.rootVisualElement.Clear();

                    ShowNextNotification();
                }
            }
        }

        private void ShowNextNotification()
        {
            lastNotification = notificationQueue.Dequeue();
            uiDocument.rootVisualElement.Add(lastNotification);
            lastNotification.Show();
        }

        private void OnQuestStarted(Quest quest)
        {
            var questStartedNotification = new QuestNotificationView(questNotificationData,
                $"Quest started: {quest.questData.QuestName}", QuestNotificationView.NotificationType.QuestStarted);
            notificationQueue.Enqueue(questStartedNotification);
        }

        private void OnTaskCompleted(Task task)
        {
            if (!task.taskData.isHidden)
            {
                var taskCompletedNotification = new QuestNotificationView(questNotificationData,
                    $"Completed: {task.taskData.description}", QuestNotificationView.NotificationType.QuestUpdate);
                notificationQueue.Enqueue(taskCompletedNotification);
            }
        }

        private void OnNewTask(Task task)
        {
            if (!task.taskData.isHidden)
            {
                var newTaskNotification = new QuestNotificationView(questNotificationData, task.taskData.description,
                    QuestNotificationView.NotificationType.QuestUpdate);
                notificationQueue.Enqueue(newTaskNotification);
            }
        }

        private void OnQuestCompleted(Quest quest)
        {
            var questCompletedNotification = new QuestNotificationView(questNotificationData,
                $"Quest completed: {quest.questData.QuestName}", QuestNotificationView.NotificationType.QuestCompleted);
            notificationQueue.Enqueue(questCompletedNotification);
        }

        private void OnQuestFailed(Quest quest)
        {
            var questFailedNotification = new QuestNotificationView(questNotificationData,
                $"Quest failed: {quest.questData.QuestName}", QuestNotificationView.NotificationType.QuestCompleted);
            notificationQueue.Enqueue(questFailedNotification);
        }

        private void OnDestroy()
        {
            questChannel.onQuestStarted -= OnQuestStarted;
            questChannel.onQuestCompleted -= OnQuestCompleted;
        }
    }
}