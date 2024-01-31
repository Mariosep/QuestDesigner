using UnityEngine;
using UnityEngine.UIElements;

/*
[RequireComponent(typeof(UIDocument))]
public class QuestJournalUIHandler : MonoBehaviour
{
    private UIDocument uiDocument;
    
    private QuestRunner questRunner;    
    private InputManager inputManager;
    private UIChannel uiChannel;
    
    private QuestListRuntimeView questListRuntimeView;
    private QuestDetailsView questDetailsView;

    [SerializeField]
    private bool isOpen;
    
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        questRunner = ServiceLocator.Get<QuestRunner>();
        inputManager = ServiceLocator.Get<InputManager>();
        uiChannel = ServiceLocator.Get<UIChannel>();
        
        Hide();

        inputManager.OnOpenQuestJournalPerformed += Toggle;
    }

    private void Toggle()
    {
        if(isOpen)
            Hide();
        else
            Show();
        
        isOpen = !isOpen;
    }   
    
    private void Show()
    {
        uiDocument.enabled = true;
        
        questListRuntimeView = uiDocument.rootVisualElement.Q<QuestListRuntimeView>();
        questDetailsView = uiDocument.rootVisualElement.Q<QuestDetailsView>();
        
        questListRuntimeView.onQuestSelected += OnQuestSelected;
        
        if(questRunner.questJournal != null)
            questListRuntimeView.Populate(questRunner.questJournal);
        
        uiChannel.onMenuOpen?.Invoke();
    }
    
    private void Hide()
    {
        uiDocument.enabled = false;
        
        uiChannel.onMenuClosed?.Invoke();
    }
    
    private void OnQuestSelected(Quest quest)
    {
        questDetailsView.Populate(quest);
    }
}*/