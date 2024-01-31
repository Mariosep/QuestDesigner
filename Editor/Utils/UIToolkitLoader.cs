using UnityEditor;
using UnityEngine.UIElements;

public static class UIToolkitLoader
{
    public static VisualTreeAsset LoadUXML(string editorWindowPath, string uxmlName)
    {
        string path = $"{editorWindowPath}/UXML/{uxmlName}";
        return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
    }
    
    public static StyleSheet LoadStyleSheet(string editorWindowPath, string styleSheetName)
    {
        string path = $"{editorWindowPath}/USS/{styleSheetName}";
        return AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
    }
}