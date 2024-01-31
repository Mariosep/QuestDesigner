using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/*public class QuestDesignerRuntimeWindow : EditorWindow
{
    public static string RelativePath => AssetDataBaseExtensions.GetDirectoryOfScript<QuestDesignerRuntimeWindow>();
    
    private readonly string uxmlName = "QuestDesignerRuntime.uxml";

    public static QuestJournalSO questJournal;
    
    private QuestToolbarView questToolbarView;
    private QuestInspectorView questInspectorView;
    private QuestSelectorRuntimeView questSelectorRuntimeView; 
    
    private QuestGraphRuntimeEditorWindow questGraphRuntimeEditorWindow;
    
    [MenuItem("Tools/Quest Designer Debug")]
    public static void OpenWindow()
    {
        QuestDesignerRuntimeWindow wnd = GetWindow<QuestDesignerRuntimeWindow>("Quest Designer Debug");
        wnd.minSize = new Vector2(800, 600);
    }
    
    public void CreateGUI()
    {
        VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(RelativePath, uxmlName);
        uxml.CloneTree(rootVisualElement);

        questJournal = ServiceLocator.Get<QuestRunner>().questJournal;
        
        // Get references
        //questToolbarView = rootVisualElement.Q<QuestToolbarView>();
        questInspectorView = rootVisualElement.Q<QuestInspectorView>();
        questSelectorRuntimeView = rootVisualElement.Q<QuestSelectorRuntimeView>();

        //questToolbarView.PopulateView(questSelectorView);
        
        questInspectorView.visible = false;
        
        questSelectorRuntimeView.onQuestSelected += OnQuestSelected;
        questSelectorRuntimeView.onQuestChosen += OnQuestChosen;
        questSelectorRuntimeView.PopulateView(questJournal);
    }
    
    private void OnQuestSelected(QuestSO questSelected)
    {
        questInspectorView.visible = true;
        
        questInspectorView.BindQuest(questSelected);
    }
    
    private void OnQuestChosen(QuestSO questChosen)
    {
        QuestGraphSO questGraph = QuestManager.instance.GetQuestGraph(questChosen);
        
        OpenQuestGraph(questGraph);
    }
    
    public void OpenQuestGraph(QuestGraphSO questGraph)
    {
        questGraphRuntimeEditorWindow = GetWindow<QuestGraphRuntimeEditorWindow>("Quest Graph Debug");
        questGraphRuntimeEditorWindow.OpenQuestGraph(questGraph);
    }
}*/