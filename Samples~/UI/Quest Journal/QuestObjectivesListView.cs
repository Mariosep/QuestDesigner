using UnityEngine;
using UnityEngine.UIElements;

/*public class QuestObjectivesListView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<QuestObjectivesListView, UxmlTraits> { }

    private VisualTreeAsset objectiveListViewAsset;
    private VisualTreeAsset objectiveItemAsset;

    private VisualElement currentObjectivesContainer;
    private VisualElement completedObjectivesContainer;
    
    private ListView currentObjectivesListView;
    private ListView completedObjectivesListView;
    

    private Quest quest;
    
    public QuestObjectivesListView()
    {
        objectiveListViewAsset = Resources.Load<VisualTreeAsset>("UXML/QuestJournal/QuestObjectiveListView");
        objectiveItemAsset = Resources.Load<VisualTreeAsset>("UXML/QuestJournal/QuestObjectiveViewItem");

        currentObjectivesContainer = new VisualElement();
        objectiveListViewAsset.CloneTree(currentObjectivesContainer);

        currentObjectivesListView = currentObjectivesContainer.Q<ListView>();
        
        completedObjectivesContainer = new VisualElement();
        objectiveListViewAsset.CloneTree(completedObjectivesContainer);
        
        completedObjectivesListView = completedObjectivesContainer.Q<ListView>();

        Add(completedObjectivesContainer);
        Add(currentObjectivesContainer);
        
        Setup();
    }

    private void Setup()
    {
        currentObjectivesListView.makeItem = MakeItem;
        currentObjectivesListView.bindItem = (element, i) => BindItem(element, quest.CurrentTasks[i]);
        
        completedObjectivesListView.makeItem = MakeItem;
        completedObjectivesListView.bindItem = (element, i) => BindItem(element, quest.CompletedTasks[i]);
    }

    public void Populate(Quest quest)
    {
        if(this.quest != null)
        {
            this.quest.onStepStarted -= OnTaskStarted;
            this.quest.onStepCompleted -= OnTaskCompleted;
        }
        
        this.quest = quest;

        currentObjectivesListView.itemsSource = quest.CurrentTasks;
        currentObjectivesListView.RefreshItems();
        OnCurrentObjectivesListUpdated();
        
        completedObjectivesListView.itemsSource = quest.CompletedTasks;
        completedObjectivesListView.RefreshItems();
        OnCompletedObjectivesListUpdated();

        quest.onStepStarted += OnTaskStarted;
        quest.onStepCompleted += OnTaskCompleted;
    }

    private void OnTaskStarted(Task taskStarted)
    {
        if(taskStarted.taskData.isHidden)
            return;
        
        currentObjectivesListView.itemsSource.Add(taskStarted);
        currentObjectivesListView.RefreshItems();
        OnCurrentObjectivesListUpdated();
    }
    
    private void OnTaskCompleted(Task taskCompleted)
    {
        if(taskCompleted.taskData.isHidden)
            return;

        currentObjectivesListView.itemsSource.Remove(taskCompleted);
        currentObjectivesListView.RefreshItems();
        OnCurrentObjectivesListUpdated();
        
        completedObjectivesListView.itemsSource.Add(taskCompleted);
        completedObjectivesListView.RefreshItems();
        OnCompletedObjectivesListUpdated();
    }
    
    private VisualElement MakeItem()
    {
        var objectiveItem = new VisualElement();
        objectiveItemAsset.CloneTree(objectiveItem);
        return objectiveItem;
    }

    private void BindItem(VisualElement element, Task task)
    {
        var taskLabel = element.Q<Label>();
        var taskStateToggle = element.Q<Toggle>();

        string taskDescription = task.taskData.description; 
        
        if(taskDescription != "")
            taskLabel.text = taskDescription;
        else
            taskLabel.text = "Empty Objective";

        taskStateToggle.value = task.taskState == Task.TaskState.Completed;
    }

    private void OnCurrentObjectivesListUpdated()
    {
        if (currentObjectivesListView.itemsSource.Count == 0)
            currentObjectivesContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        else
            currentObjectivesContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
    }
    
    private void OnCompletedObjectivesListUpdated()
    {
        if (completedObjectivesListView.itemsSource.Count == 0)
            completedObjectivesContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        else
            completedObjectivesContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
    }
}*/