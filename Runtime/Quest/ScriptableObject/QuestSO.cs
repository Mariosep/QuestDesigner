using System;
using Blackboard.Commands;
using Unity.Properties;
using UnityEngine;

namespace QuestDesigner
{
    public class QuestSO : ScriptableObject
    {
        public Action onNameChanged;
        public Action<bool> onFailableValueChanged;
        public Action<QuestSO> onNextQuestChanged;

        public string id;
        [SerializeField, DontCreateProperty] private string questName;
        public string description;

        public StepSO initialStep;

        public bool enableOnStart = true;
        public bool failable = false;

        private QuestSO nextQuest;

        public CommandList onStart;
        public CommandList onComplete;

        [CreateProperty]
        public string QuestName
        {
            get => questName;
            set
            {
                //var questDataBase = QuestManager.instance.QuestDataBase;
                //string validName = QuestValidator.GetValidName(questDataBase, nameField.value, quest);
                questName = value;
                
                onNameChanged?.Invoke();
            }
        }

        public bool Failable
        {
            get => failable;
            set
            {
                failable = value;
                onFailableValueChanged?.Invoke(failable);
            }
        }

        public QuestSO NextQuest
        {
            get => nextQuest;
            set
            {
                nextQuest = value;
                onNextQuestChanged?.Invoke(nextQuest);
            }
        }

        public void Init(string id)
        {
            this.id = id;
            name = $"quest-{id}";
            questName = "NewQuest";
            onStart = CreateInstance<CommandList>();
            onComplete = CreateInstance<CommandList>();

            onStart.Init(id);
            onComplete.Init(id);
        }
    }
}