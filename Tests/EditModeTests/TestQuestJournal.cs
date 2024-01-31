using NSubstitute;
using NUnit.Framework;

/*public class TestQuestJournal
{
    private QuestJournal questJournal;
    
    [SetUp]
    public void SetUp()
    {
        questJournal = new QuestJournal();
    }
    
    [Test]
    public void WhenQuestCompleted_ThenAddQuestToCompletedQuests()
    {
        // Arrange
        IQuest quest = Substitute.For<IQuest>();
        quest.CheckRequirements().Returns(true);
        questJournal.AssignQuest(quest);
        
        // Act
        QuestChannel.QuestCompleted(quest);
        
        // Assert
        Assert.IsTrue(questJournal.CompletedQuests.Contains(quest), "Completed Quest has not been added to CompletedQuests");
    }
    
    [Test]
    public void WhenQuestAssigned_IfRequirementsFulfilled_ThenAddQuestToCurrentQuests()
    {
        // Arrange
        IQuest quest = Substitute.For<IQuest>();
        quest.CheckRequirements().Returns(true);
        
        // Act
        questJournal.AssignQuest(quest);
        
        // Assert
        Assert.IsTrue(questJournal.CurrentQuests.Contains(quest), "Quest has not been added to CurrentQuests");
    }
    
    [Test]
    public void WhenQuestAssigned_IfRequirementsNotFulfilled_ThenAddQuestToPendingQuests()
    {
        // Arrange
        IQuest quest = Substitute.For<IQuest>();
        quest.CheckRequirements().Returns(false);
        
        // Act
        questJournal.AssignQuest(quest);
        
        // Assert
        Assert.IsTrue(questJournal.PendingQuests.Contains(quest), "Quest has not been added to PendingQuests");
    }
    
    [Test]
    public void WhenQuestAssigned_IfRequirementsFulfilled_ThenStartQuest()
    {
        // Arrange
        IQuest quest = Substitute.For<IQuest>();
        quest.CheckRequirements().Returns(true);
        
        // Act
        questJournal.AssignQuest(quest);
        
        // Assert
        quest.Received().Start();
    }
    
    [Test]
    public void WhenQuestAssigned_IfRequirementsNotFulfilled_ThenDontStartQuest()
    {
        // Arrange
        IQuest quest = Substitute.For<IQuest>();
        quest.CheckRequirements().Returns(false);
        
        // Act
        questJournal.AssignQuest(quest);
        
        // Assert
        quest.Received(0).Start();
    }
}*/
