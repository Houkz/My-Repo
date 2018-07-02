namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests
{
    public interface IQuestSubscriber
    {
        // Do the AddListener call to the QuestSubscriptionHandler through the manager services.
        void Start();
        // Function is used for removing a listener in the QuestSubscriptionHandler through the manager services.
        void OnDestroy();
        // Use this function to react on incomming quest updates. Look at already existing questsubscribers for reference.
        void Notify(Quest questToValidate);
        // This function updates the inspector. Look at already existing questsubscribers for reference.
        void OnValidate();
    }
}
