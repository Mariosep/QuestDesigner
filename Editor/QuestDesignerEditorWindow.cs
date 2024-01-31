using Blackboard.Utils.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public class QuestDesignerEditorWindow : EditorWindow
    {
        public static string RelativePath => AssetDataBaseExtensions.GetDirectoryOfScript<QuestDesignerEditorWindow>();

        private readonly string uxmlName = "QuestDesignerEditor.uxml";

        public static QuestDataBase questDataBase;

        private QuestToolbarView questToolbarView;
        private QuestEditorInspectorView questEditorInspectorView;

        private Button editorTabButton;
        private Button runtimeTabButton;

        private VisualElement rightPanel;

        private enum QuestDesignerTab
        {
            None,
            Editor,
            Runtime
        }

        private QuestDesignerTab questDesignerTabSelected;

        [MenuItem("Tools/Quest Designer")]
        public static QuestDesignerEditorWindow OpenWindow()
        {
            return GetWindow<QuestDesignerEditorWindow>("Quest Designer");
        }

        public void CreateGUI()
        {
            VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(RelativePath, uxmlName);
            uxml.CloneTree(rootVisualElement);

            titleContent.text = "Quest Designer";
            
            minSize = new Vector2(800, 600);

            questDataBase = QuestManager.instance.QuestDataBase;

            questDesignerTabSelected = QuestDesignerTab.None;

            // Get references
            questToolbarView = rootVisualElement.Q<QuestToolbarView>();
            questEditorInspectorView = rootVisualElement.Q<QuestEditorInspectorView>();

            editorTabButton = rootVisualElement.Q<Button>("editor__tab-button");
            runtimeTabButton = rootVisualElement.Q<Button>("runtime__tab-button");

            rightPanel = rootVisualElement.Q<VisualElement>("right-panel");

            ShowEditorTab();

            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            editorTabButton.clicked += OnEditorTabButtonClicked;
            runtimeTabButton.clicked += OnRuntimeTabButtonClicked;
        }

        private void UnregisterCallbacks()
        {
            editorTabButton.clicked -= OnEditorTabButtonClicked;
            runtimeTabButton.clicked -= OnRuntimeTabButtonClicked;
        }

        private void OnEditorTabButtonClicked() => ShowEditorTab();
        private void OnRuntimeTabButtonClicked() => ShowRuntimeTab();

        public void ShowEditorTab()
        {
            if (questDesignerTabSelected == QuestDesignerTab.Editor)
                return;

            runtimeTabButton.RemoveFromClassList("tab-button--selected");
            editorTabButton.AddToClassList("tab-button--selected");

            questEditorInspectorView.UnbindQuest();

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            questDesignerTabSelected = QuestDesignerTab.Editor;

            var questSelectorView = new QuestSelectorView();
            questSelectorView.onQuestSelected += OnQuestSelected;
            questSelectorView.onQuestChosen += OnQuestChosen;
            questSelectorView.PopulateView(questDataBase);

            rightPanel.RemoveAt(1);
            rightPanel.Insert(1, questSelectorView);
        }

        public void ShowRuntimeTab()
        {
            if (questDesignerTabSelected == QuestDesignerTab.Runtime)
                return;

            editorTabButton.RemoveFromClassList("tab-button--selected");
            runtimeTabButton.AddToClassList("tab-button--selected");

            questEditorInspectorView.UnbindQuest();

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            questDesignerTabSelected = QuestDesignerTab.Runtime;

            var questSelectorRuntimeView = new QuestSelectorRuntimeView();

            questSelectorRuntimeView.onQuestSelected += OnQuestSelected;
            questSelectorRuntimeView.onQuestChosen += OnQuestChosen;

            if (EditorApplication.isPlaying)
            {
                QuestJournalSO questJournal = ServiceLocator.Get<QuestRunner>().questJournal;
                if (questJournal != null)
                    questSelectorRuntimeView.PopulateView(questJournal);
            }

            rightPanel.RemoveAt(1);
            rightPanel.Insert(1, questSelectorRuntimeView);
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            var questSelectorRuntimeView = rightPanel.Q<QuestSelectorRuntimeView>();

            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    questSelectorRuntimeView.ShowEditModeHelpBox();
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    QuestJournalSO questJournal = ServiceLocator.Get<QuestRunner>().questJournal;
                    if (questJournal != null)
                        questSelectorRuntimeView.PopulateView(questJournal);
                    break;
            }
        }

        private void OnQuestSelected(QuestSO questSelected)
        {
            questEditorInspectorView.BindQuest(questSelected);
        }

        private void OnQuestChosen(QuestSO questChosen)
        {
            QuestGraphSO questGraph = QuestManager.instance.GetQuestGraph(questChosen);

            OpenQuestGraph(questGraph);
        }

        public void OpenQuestGraph(QuestGraphSO questGraph)
        {
            var questGraphEditorWindow = GetWindow<QuestGraphEditorWindow>("Quest Graph Editor");
            questGraphEditorWindow.OpenQuestGraph(questGraph);
        }

        private void OnDestroy()
        {
            UnregisterCallbacks();

            if (questDataBase != null)
            {
                EditorUtility.SetDirty(questDataBase);
                AssetDatabase.SaveAssets();
            }
        }
    }
}