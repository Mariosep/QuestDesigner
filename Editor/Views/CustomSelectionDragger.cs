using System;
using System.Collections.Generic;
using QuestDesigner.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomSelectionDragger : SelectionDragger
{
    public Action<List<NodeView>, List<Rect>, List<Rect>> onDragNodeEnded;

    private QuestGraphView questGraphView;

    private List<NodeView> nodesList;
    private List<Rect> oldPositionsList;

    public CustomSelectionDragger(QuestGraphView questGraphView)
    {
        this.questGraphView = questGraphView;

        nodesList = new List<NodeView>();
        oldPositionsList = new List<Rect>();
    }
    
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(CacheOriginalPosition);
        target.RegisterCallback<MouseUpEvent>(RegisterMoveNodeCommand);

        base.RegisterCallbacksOnTarget();
    }

    private void CacheOriginalPosition(MouseDownEvent evt)
    {
        nodesList = new List<NodeView>();
        oldPositionsList = new List<Rect>();
        
        var nodesSelected = questGraphView.selection;
        
        if(nodesSelected.Count == 0)
            return;

        foreach (ISelectable selectable in nodesSelected)
        {
            if (selectable is NodeView nodeView)
            {
                nodesList.Add(nodeView);
                oldPositionsList.Add(nodeView.GetPosition());
            }
        }
    }
    
    private void RegisterMoveNodeCommand(MouseUpEvent evt)
    {
        if(nodesList.Count == 0)
            return;
        
        var newPositionsList = new List<Rect>();
        
        foreach (NodeView nodeView in nodesList)   
            newPositionsList.Add(nodeView.GetPosition());
        
        onDragNodeEnded?.Invoke(nodesList, oldPositionsList, newPositionsList);    
    }
}