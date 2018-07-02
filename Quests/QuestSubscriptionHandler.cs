using DownTheWell.Utility.ServiceLocator;
using System.Collections.Generic;
using DownTheWell.Gameplay.World.Quests.SubscriberQuests;

namespace DownTheWell.Gameplay.World.Quests
{
    public class QuestSubscriptionHandler : Service<ManagerServices>
    {
        private List<IQuestSubscriber> subscribers;
        private List<IQuestSubscriber> subscribersToRemove;

        protected override void Awake()
        {
            base.Awake();
            subscribers = new List<IQuestSubscriber>();
            subscribersToRemove = new List<IQuestSubscriber>();
        }

        public void NotifySubscribers(Quest updatedQuest)
        {
            foreach (IQuestSubscriber subscriber in subscribers)
            {
                subscriber.Notify(updatedQuest);
            }

            foreach (IQuestSubscriber subscriberToRemove in subscribersToRemove)
            {
                subscribers.Remove(subscriberToRemove);
            }

            for (int i = 0; i < subscribersToRemove.Count;)
            {
                subscribersToRemove.RemoveAt(i);
            }
        }

        public void AddSubscriber(IQuestSubscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void RemoveSubscriber(IQuestSubscriber subscriber)
        {
            subscribersToRemove.Add(subscriber);
        }
    }
}
