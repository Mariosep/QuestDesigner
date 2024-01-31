using UnityEngine;
using UnityEngine.UIElements;

/*
public class QuestDetailsView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<QuestDetailsView, UxmlTraits> { }

    private Quest quest;

    private Label questName;
    private Label questDescription;
    
    private QuestObjectivesListView questObjectivesListView;

    public QuestDetailsView()
    {
        var uxml = Resources.Load<VisualTreeAsset>("UXML/QuestJournal/QuestDetailsView");
        uxml.CloneTree(this);

        Setup();
    }

    private void Setup()
    {
        questName = this.Q<Label>("quest-details__name__label");
        questDescription = this.Q<Label>("quest-details__description__label");
        questObjectivesListView = this.Q<QuestObjectivesListView>();
    } 
    
    public void Populate(Quest quest)
    {
        this.quest = quest;

        questName.text = quest.questData.questName;
        questDescription.text = quest.questData.description;
        
        questObjectivesListView.Populate(quest);
    }
}*/