namespace QuestDesigner
{
    public class QuestRunner : Singleton<QuestRunner>
    {
        public QuestJournalSO questJournal;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            if (questJournal != null)
                questJournal.Init();
        }

        private void Update()
        {
            if (questJournal != null)
                questJournal.Update();
        }
    }
}