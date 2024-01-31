using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Quest/QuestNotification", fileName = "QuestNotification")]
public class QuestNotificationSO : ScriptableObject
{
    public VisualTreeAsset questNotificationUxml;
    public float fadeInDuration;
    public float fullyVisibleDuration;
    public float fadeOutDuration;
    public AudioClip onStartNotificationClip;
}




