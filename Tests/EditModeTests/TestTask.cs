using NSubstitute;
using NUnit.Framework;
using UnityEngine;

/*public class TestTask
{
    private TaskOld task;
    
    [SetUp]
    public void SetUp()
    {
        TaskData taskData = ScriptableObject.CreateInstance<TaskData>();
        task = new EmptyTaskOld(taskData);
    }
    
    [Test]
    public void WhenTaskStarted_ThenChangeTaskStateToInProgress()
    {
        // Act
        task.Start();
        
        // Assert
        Assert.AreEqual(TaskState.InProgress, task.State, "Task state is not equal to InProgress");
    }
    
    [Test]
    public void WhenTaskCompleted_ThenChangeTaskStateToCompleted()
    {
        // Act
        task.Complete();
        
        // Assert
        Assert.AreEqual(TaskState.Completed, task.State, "Task state is not equal to Completed");
    }
    
    
    [Test]
    public void WhenTaskStarted_ThenExecuteOnTaskStartedActions()
    {
        // Arrange
        IAction action = Substitute.For<IAction>();
        task.AddOnTaskStartedAction(action);
        
        // Act
        task.Start();

        // Assert
        action.Received().Execute();
    }
    
    [Test]
    public void WhenTaskCompleted_ThenExecuteOnTaskCompletedActions()
    {
        // Arrange
        IAction action = Substitute.For<IAction>();
        task.AddOnTaskCompletedAction(action);
        
        // Act
        task.Complete();

        // Assert
        action.Received().Execute();
    }
    
    [Test]
    public void WhenTaskStarted_ThenInvokeOnTaskStartedEvent()
    {
        // Arrange
        var eventChecker = new EventInvokedChecker();
        QuestChannel.onTaskStarted += eventChecker.OnEventInvoked;
        
        // Act
        task.Start();

        // Assert
        Assert.IsTrue(eventChecker.eventHasBeenInvoked, "OnTaskStartedEvent has not been invoked");
    }
    
    [Test]
    public void WhenTaskCompleted_ThenInvokeOnTaskCompletedEvent()
    {
        // Arrange
        var eventChecker = new EventInvokedChecker();
        QuestChannel.onTaskCompleted += eventChecker.OnEventInvoked;
        
        // Act
        task.Complete();

        // Assert
        Assert.IsTrue(eventChecker.eventHasBeenInvoked, "OnTaskCompletedEvent has not been invoked");
    }
}*/