/*using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestGraphRuntimeEditorWindow : EditorWindow
{
    public static string RelativePath => AssetDataBaseExtensions.GetDirectoryOfScript<QuestGraphRuntimeEditorWindow>();
    
    private readonly string uxmlName = "QuestGraphDebugEditor.uxml";

    public static QuestGraphSO questGraph;

    //private QuestToolbarView questToolbarView;
    private GraphInspectorView graphInspectorView;
    //private QuestGraphRuntimeView questGraphRuntimeView;
    
    public void OpenQuestGraph(QuestGraphSO questGraphToOpen)
    {
        questGraph = questGraphToOpen;
        
        //questGraphRuntimeView.PopulateView(questGraph);
        graphInspectorView.BindQuest(questGraph.quest);
    }
    
    public void CreateGUI()
    {
        VisualTreeAsset uxml = UIToolkitLoader.LoadUXML(RelativePath, uxmlName);
        uxml.CloneTree(rootVisualElement);
        
        minSize = new Vector2(800, 600);
        
        // Get references
        //questToolbarView = rootVisualElement.Q<QuestToolbarView>();
        graphInspectorView = rootVisualElement.Q<GraphInspectorView>();
    }
    
    private void OnNodeSelected(NodeView nodeView)
    {
        graphInspectorView.BindNode(nodeView.node);
    }

    private void OnNodeUnselected(NodeView nodeView)
    {
        graphInspectorView.UnbindNode();
    }
}*/