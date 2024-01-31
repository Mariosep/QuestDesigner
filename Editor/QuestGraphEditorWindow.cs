using Blackboard.Utils.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestGraphEditorWindow : EditorWindow
    {
        public static string RelativePath => AssetDataBaseExtensions.GetDirectoryOfScript<QuestGraphEditorWindow>();

        private readonly string uxmlName = "QuestGraphEditor.uxml";

        public static QuestGraphSO questGraph;

        private GraphInspectorView graphInspectorView;
        private QuestGraphView questGraphView;

        private bool loadLastQuestGraph;

        public void OpenQuestGraph(QuestGraphSO questGraphToOpen)
        {
            questGraph = questGraphToOpen;

            questGraphView.PopulateView(questGraph);
            graphInspectorView.BindQuest(questGraph.quest);

            EditorPrefs.SetString("questGraphId", questGraphToOpen.id);

            if (EditorApplication.isPlaying)
            {
                QuestJournalSO questJournal = QuestRunner.Instance.questJournal;
                ShowDebugGraph(questJournal);
            }
            else
                ShowEditorGraph();
        }

        public void CreateGUI()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(RelativePath, uxmlName);
            uxml.CloneTree(rootVisualElement);

            titleContent.text = "Quest Graph";
            
            minSize = new Vector2(600, 300);

            // Get references
            graphInspectorView = rootVisualElement.Q<GraphInspectorView>();
            questGraphView = rootVisualElement.Q<QuestGraphView>();

            questGraphView.onNodeSelected += OnNodeSelected;
            questGraphView.onNodeUnselected += OnNodeUnselected;
            questGraphView.SetEditorWindow(this);

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            if (loadLastQuestGraph)
            {
                string questGraphToLoadId = EditorPrefs.GetString("questGraphId");
                questGraph = QuestManager.instance.GetQuestGraph(questGraphToLoadId);

                OpenQuestGraph(questGraph);
            }

            loadLastQuestGraph = true;
        }

        private void ShowEditorGraph()
        {
            questGraphView.DisableDebugMode();
        }

        private void ShowDebugGraph(QuestJournalSO questJournal)
        {
            questGraphView.EnableDebugMode(questJournal);
        }
        
        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    QuestJournalSO.onQuestJournalLoaded -= ShowDebugGraph;
                    ShowEditorGraph();
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    QuestJournalSO.onQuestJournalLoaded += ShowDebugGraph;
                    QuestJournalSO questJournal = QuestRunner.Instance.questJournal;
                    ShowDebugGraph(questJournal);
                    break;
            }
        }

        private void OnNodeSelected(NodeView nodeView)
        {
            graphInspectorView.BindNode(nodeView.node);
        }

        private void OnNodeUnselected(NodeView nodeView)
        {
            graphInspectorView.UnbindNode();
        }

        private void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            QuestJournalSO.onQuestJournalLoaded -= ShowDebugGraph;

            loadLastQuestGraph = false;

            if (questGraph != null)
            {
                EditorUtility.SetDirty(questGraph);

                questGraph = null;
            }

            AssetDatabase.SaveAssets();
        }
    }
}