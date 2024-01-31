using System;
using System.Collections.Generic;
using Blackboard.Editor;

namespace QuestDesigner.Editor
{
    public class QuestSearchProvider : DataSearchProvider<QuestSO>
    {
        public override void Init(List<KeyValuePair<QuestSO, string>> quests, Action<QuestSO> callback)
        {
            base.Init(quests, callback);

            searchTreeTitle = "Quests";
        }
    }
}