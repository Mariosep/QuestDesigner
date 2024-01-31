using System.Collections.Generic;
using UnityEngine;

namespace QuestDesigner
{
    public abstract class StepSO : ScriptableObject
    {
        public string id;

        public List<StepSO> nextSteps = new List<StepSO>();

        public bool completeQuest = false;
        public bool failQuest = false;

        public abstract void Init(string id);
    }
}