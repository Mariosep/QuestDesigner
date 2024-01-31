using Blackboard.Commands;
using Blackboard.Requirement;
using UnityEditor;
using UnityEngine;

namespace QuestDesigner.Editor
{
    public class TaskNodeSO : NodeSO
    {
        public TaskSO task;

        public override void Init(Vector2 position)
        {
            base.Init(position);

            name = $"taskNode-{id}";
            task = ScriptableObject.CreateInstance<TaskSO>();
            task.Init(id);

            step = task;
        }

        public override NodeSO Clone()
        {
            TaskNodeSO clonedTaskNode = (TaskNodeSO)base.Clone();

            string newId = clonedTaskNode.id;
            clonedTaskNode.name = $"taskNode-{newId}";

            clonedTaskNode.task = task.Clone(newId);

            return clonedTaskNode;
        }

        public override void SaveAs(QuestGraphSO questGraph)
        {
            base.SaveAs(questGraph);

            EditorUtility.SetDirty(this);

            AssetDatabase.AddObjectToAsset(task, questGraph);
            AssetDatabase.AddObjectToAsset(task.requirements, questGraph);

            foreach (ConditionSO cond in task.requirements.conditions)
                AssetDatabase.AddObjectToAsset(cond, questGraph);

            AssetDatabase.AddObjectToAsset(task.onStart, questGraph);
            AssetDatabase.AddObjectToAsset(task.onComplete, questGraph);

            foreach (Command action in task.onStart.commands)
                AssetDatabase.AddObjectToAsset(action, questGraph);
            
            foreach (Command action in task.onComplete.commands)
                AssetDatabase.AddObjectToAsset(action, questGraph);

            AssetDatabase.SaveAssets();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);

                foreach (ConditionSO cond in task.requirements.conditions)
                    AssetDatabase.RemoveObjectFromAsset(cond);

                AssetDatabase.RemoveObjectFromAsset(task.requirements);

                foreach (Command command in task.onStart.commands)
                    AssetDatabase.RemoveObjectFromAsset(command);
                
                foreach (Command command in task.onComplete.commands)
                    AssetDatabase.RemoveObjectFromAsset(command);

                AssetDatabase.RemoveObjectFromAsset(task.onStart);
                AssetDatabase.RemoveObjectFromAsset(task.onComplete);
                AssetDatabase.RemoveObjectFromAsset(task);
                AssetDatabase.SaveAssets();
            }
        }
    }
}