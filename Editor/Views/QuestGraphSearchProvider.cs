using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public class QuestGraphSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private QuestGraphView questGraphView;
        private Texture2D _indentationIcon;

        public void Initialize(QuestGraphView questGraphView)
        {
            this.questGraphView = questGraphView;

            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, Color.clear);
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node")),
                new SearchTreeEntry(new GUIContent("Task", _indentationIcon))
                {
                    level = 1,
                    userData = NodeType.Task
                },
                new SearchTreeEntry(new GUIContent("Condition", _indentationIcon))
                {
                    level = 1,
                    userData = NodeType.Branch
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = questGraphView.GetLocalMousePosition(context.screenMousePosition, true);

            NodeType nodeType = (NodeType)SearchTreeEntry.userData;

            questGraphView.CreateNode(nodeType, localMousePosition);

            return true;
        }
    }
}