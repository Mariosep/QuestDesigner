using Blackboard.Events;
using Blackboard.Interactions;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionUIHandler : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label interactionLabel;

    private InteractionEventChannel interactionChannel;

    private string interactButtonName;
    
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        uiDocument.enabled = false;

        interactionChannel = ServiceLocator.Get<InteractionEventChannel>();
        
        interactionChannel.onInteractableTriggerEnter += OnInteractableTriggerEnter;
        interactionChannel.onInteractableTriggerExit += OnInteractableTriggerExit;
    }

    private void OnInteractableTriggerEnter(IInteractable interactable)
    {
        uiDocument.enabled = true;
        interactionLabel = uiDocument.rootVisualElement.Q<Label>();
        
        interactionLabel.text = $"E) {interactable.InteractionName}\n{interactable.Name}";
    }
    
    private void OnInteractableTriggerExit(IInteractable interactable)
    {
        uiDocument.enabled = false;
    }
}
