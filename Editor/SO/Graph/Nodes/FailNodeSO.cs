using UnityEngine;

namespace QuestDesigner.Editor
{
    public class FailNodeSO : NodeSO
    {
        public override void Init(Vector2 position)
        {
            base.Init(position);

            this.name = "Fail";
        }
    }
}