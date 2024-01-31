using UnityEngine;
using UnityEngine.UIElements;

public class QuestNotificationView : VisualElement
{
    public enum NotificationType
    {
        QuestStarted,
        QuestUpdate,
        QuestCompleted
    }

    public NotificationType type;
    public bool isCompleted;

    private Label notificationLabel;
    
    private string text;
    private QuestNotificationSO data;

    //private Sequence sequence;


    public QuestNotificationView(QuestNotificationSO data, string text, NotificationType type)
    {
        this.data = data;
        this.text = text;
        this.type = type;
        
        data.questNotificationUxml.CloneTree(this);
        
        notificationLabel = this.Q<Label>();
        this.style.opacity = 0;
    }

    public void Show()
    {
        this.style.opacity = 0;
        
        notificationLabel.text = text;

        /*sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => 0f, x => style.opacity = x, 1f, data.fadeInDuration).SetEase(Ease.Linear));
        sequence.AppendInterval(data.fullyVisibleDuration);
        sequence.Append(DOTween.To(() => 1f, x => style.opacity = x, 0f, data.fadeOutDuration).SetEase(Ease.Linear));
        sequence.onComplete = () => isCompleted = true;
        sequence.Play();*/

        if (data.onStartNotificationClip != null)
            AudioSource.PlayClipAtPoint(this.data.onStartNotificationClip, Vector3.zero);
    }
}