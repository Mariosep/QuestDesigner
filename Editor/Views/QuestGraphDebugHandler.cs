using UnityEditor.Experimental.GraphView;

namespace QuestDesigner.Editor
{
    public class QuestGraphDebugHandler
    {
        private Quest _quest;

        private QuestGraphView _questGraphView;

        private QuestGraphSO _questGraph => _questGraphView.questGraph;

        public QuestGraphDebugHandler(QuestGraphView questGraphView)
        {
            _questGraphView = questGraphView;
        }

        public void EnableDebugMode(QuestJournalSO questJournal)
        {
            _quest = questJournal.GetQuest(_questGraph.id);

            foreach (var node in _questGraphView.nodes)
            {
                NodeView nodeView = (NodeView)node;
                nodeView.capabilities &= ~Capabilities.Deletable;
                
                nodeView.UpdateState(StepState.Pending);
            }

            // Update node states
            OnQuestStarted(_quest);

            foreach (Step step in _quest.CompletedTasks)
                OnStepCompleted(step);

            foreach (Step step in _quest.CurrentTasks)
                OnStepStarted(step);

            SubscribeToTaskStateEvents();
        }

        public void DisableDebugMode()
        {
            foreach (var node in _questGraphView.nodes)
            {
                NodeView nodeView = (NodeView)node;

                nodeView.UpdateState(StepState.Pending);
                nodeView.OnNodeExited();

                if (nodeView.node is StartNodeSO || nodeView.node is CompleteNodeSO || nodeView.node is FailNodeSO)
                    continue;

                nodeView.capabilities |= Capabilities.Deletable;
            }
            UnsubscribeToTaskStateEvents();
        }

        private void SubscribeToTaskStateEvents()
        {
            _quest.onStepStarted += OnStepStarted;
            _quest.onStepCompleted += OnStepCompleted;
            _quest.onQuestCompleted += OnQuestCompleted;
        }

        private void UnsubscribeToTaskStateEvents()
        {
            if(_quest != null)
            {
                _quest.onStepStarted -= OnStepStarted;
                _quest.onStepCompleted -= OnStepCompleted;
                _quest.onQuestCompleted -= OnQuestCompleted;
            }
        }
        
        private void OnStepStarted(Step stepStarted)
        {
            NodeView nodeView = _questGraphView.FindNodeView(stepStarted.stepData.id);
            nodeView.UpdateState(StepState.InProgress);
            nodeView.OnNodeEntered(stepStarted);
        }

        private void OnStepCompleted(Step stepCompleted)
        {
            NodeView nodeView = _questGraphView.FindNodeView(stepCompleted.stepData.id);
            nodeView.UpdateState(StepState.Completed);
            nodeView.OnNodeExited();
        }

        private void OnQuestStarted(Quest questStarted)
        {
            NodeView startNode = _questGraphView.FindNodeView(_questGraph.startNode);
            startNode.UpdateState(StepState.Completed);
        }

        private void OnQuestCompleted(Quest questStarted)
        {
            NodeView completeNode = _questGraphView.FindNodeView(_questGraph.completeNode);
            completeNode.UpdateState(StepState.InProgress);
        }
    }
}