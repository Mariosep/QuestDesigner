using NSubstitute;
using NUnit.Framework;

/*public class TestQuest
{
    private QuestOld quest;
    
    [SetUp]
    public void SetUp()
    {
        ITask task1 = Substitute.For<ITask>();
        ITask task2 = Substitute.For<ITask>();
        ITask task3 = Substitute.For<ITask>();
        
        quest = new QuestOld();
        
        quest.AddTask(task1);
        quest.AddTask(task2);
        quest.AddTask(task3);
    }

    [Test]
    public void WhenQuestStarted_ThenChangeQuestStateToInProgress()
    {
        // Act
        quest.Start();

        // Assert
        Assert.AreEqual(QuestState.InProgress, quest.State, "Quest state is not equal to InProgress");
    }
    
    [Test]
    public void WhenQuestCompleted_ThenChangeQuestStateToCompleted()
    {
        // Act
        quest.Complete();

        // Assert
        Assert.AreEqual(QuestState.Completed, quest.State, "Quest state is not equal to Completed");
    }
    
    [Test]
    public void WhenQuestStarted_ThenStartFirstTaskFromPendingTasks()
    {
        // Arrange
        ITask task = quest.PendingTasks[0];
        
        // Act
        quest.Start();
        
        // Assert
        task.Received().Start();
    }
    
    [Test]
    public void WhenTaskCompleted_ThenAddTaskToCompletedTasks()
    {
        // Arrange
        ITask task = quest.PendingTasks[0];
        
        // Act
        quest.Start();
        QuestChannel.onTaskCompleted(task);
        
        // Assert
        
        Assert.IsTrue(quest.CompletedTasks.Contains(task), "Completed task has not been added to completed tasks"); 
    }
    
    [Test]
    public void WhenTaskCompleted_ThenStartNextTaskFromPendingTasks()
    {
        // Arrange
        ITask task1 = quest.PendingTasks[0];
        ITask task2 = quest.PendingTasks[1];
        
        // Act
        quest.Start();
        QuestChannel.onTaskCompleted(task1);
        
        // Assert
        task2.Received().Start();
    }
    
    [Test]
    public void WhenThereAreNoMorePendingTasksAndCurrentTasksAreCompleted_ThenCompleteQuest()
    {
        // Arrange
        ITask task1 = quest.PendingTasks[0];
        ITask task2 = quest.PendingTasks[1];
        ITask task3 = quest.PendingTasks[2];
        
        // Act
        quest.Start();
        QuestChannel.TaskCompleted(task1);
        QuestChannel.TaskCompleted(task2);
        QuestChannel.TaskCompleted(task3);
        
        // Assert
        Assert.AreEqual(QuestState.Completed, quest.State, "Quest state is not equal to Completed");
    }
    
    [Test]
    public void WhenTaskAdded_ThenAddTaskToPendingTasks()
    {
        // Arrange
        ITask newTask = Substitute.For<ITask>();
        
        // Act
        quest.AddTask(newTask);

        // Assert
        Assert.IsTrue(quest.PendingTasks.Contains(newTask), "New task has not been added to pendingTasks");
    }
    
    [Test]
    public void WhenQuestStarted_ThenExecuteOnQuestStartedActions()
    {
        // Arrange
        IAction action = Substitute.For<IAction>();
        quest.AddOnQuestStartedAction(action);
        
        // Act
        quest.Start();

        // Assert
        action.Received().Execute();
    }
    
    [Test]
    public void WhenQuestCompleted_ThenExecuteOnQuestCompletedActions()
    {
        // Arrange
        IAction action = Substitute.For<IAction>();
        quest.AddOnQuestCompletedAction(action);
        
        // Act
        quest.Complete();

        // Assert
        action.Received().Execute();
    }
    
    [Test]
    public void WhenQuestStarted_ThenInvokeOnQuestStartedEvent()
    {
        // Arrange
        var eventChecker = new EventInvokedChecker();
        QuestChannel.onQuestStarted += eventChecker.OnEventInvoked;
        
        // Act
        quest.Start();

        // Assert
        Assert.IsTrue(eventChecker.eventHasBeenInvoked, "OnQuestStartedEvent has not been invoked");
    }
    
    [Test]
    public void WhenQuestCompleted_ThenInvokeOnQuestCompletedEvent()
    {
        // Arrange
        var eventChecker = new EventInvokedChecker();
        QuestChannel.onQuestCompleted += eventChecker.OnEventInvoked;
        
        // Act
        quest.Complete();

        // Assert
        Assert.IsTrue(eventChecker.eventHasBeenInvoked, "OnQuestCompletedEvent has not been invoked");
    }
}*/
