using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public class MultiTaskNodeSO : NodeSO
    {
        public List<TaskSO> tasksList;

        public override void Init(Vector2 position)
        {
            base.Init(position);

            this.name = $"multiTaskNode-{id}";

            tasksList = new List<TaskSO>();
        }

        public void AddTask(TaskSO task)
        {
            Undo.RecordObject(this, "MultiTask (Add task)");

            if (!tasksList.Contains(task))
                tasksList.Add(task);

            EditorUtility.SetDirty(this);
        }

        public void RemoveTask(TaskSO task)
        {
            Undo.RecordObject(this, "MultiTask (Remove task)");

            if (tasksList.Contains(task))
                tasksList.Remove(task);

            EditorUtility.SetDirty(this);
        }

        /*public override NodeSO Clone()
    {
        TaskNodeSO clonedTaskNode = (TaskNodeSO) base.Clone();

        string newId = clonedTaskNode.id;
        clonedTaskNode.name = $"taskNode-{newId}";
        
        clonedTaskNode.task = task.Clone(newId); 
        
        return clonedTaskNode;
    }*/
    }
}