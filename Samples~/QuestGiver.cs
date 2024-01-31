using Blackboard.Interactions;
using UnityEngine;

namespace QuestDesigner
{
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        public string npcName = "NPC";

        public QuestSO questData;
        public GameObject questMarkPrefab;
        public bool questHasBeenGiven;

        private QuestRunner questRunner;
        private GameObject questMarkInstance;

        public string Name => npcName;
        public string InteractionName => "Talk";
        public bool InteractionEnabled => !questHasBeenGiven;

        private void Start()
        {
            questRunner = ServiceLocator.Get<QuestRunner>();

            if (questMarkPrefab != null)
                questMarkInstance = Instantiate(questMarkPrefab, transform);
        }

        public bool CanInteract()
        {
            return InteractionEnabled;
        }

        [ContextMenu("Interact")]
        public void Interact()
        {
            Debug.Log("hola");
            questRunner.questJournal.AddQuest(questData);

            questHasBeenGiven = true;

            if (questMarkInstance != null)
                Destroy(questMarkInstance);
        }
    }
}