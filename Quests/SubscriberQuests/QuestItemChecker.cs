using DownTheWell.Utility.ServiceLocator;
using DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates;
using System.Collections.Generic;
using DownTheWell.Gameplay.UI.ItemSystem;
using UnityEngine;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests
{
    public class QuestItemChecker : MonoBehaviour, IQuestSubscriber
    {
        [SerializeField]
        public List<ItemCheckerQuestState> questStates;

        private QuestNotifier questNotifier;
        private ItemManager itemManager;

        public void Start()
        {
            ManagerServices.Instance.GetService<QuestSubscriptionHandler>().AddSubscriber(this);
            itemManager = ManagerServices.Instance.GetService<ItemManager>();

            questNotifier = GetComponent<QuestNotifier>();

            List<Quest> questList = QuestCollection.GetInstance().quests;
            foreach (Quest quest in questList)
            {
                Notify(quest);
            }
        }

        // @TODO:
        // Somehow notify this class (and all other Item Quests) from
        // ItemManager.Inventory.AddItem/DropItem instead of using Update()
        public void Update()
        {
            for (int i = 0; i < questStates.Count; i++)
            {
                List<ItemToCheck> items = questStates[i].itemsToCheck;
                for (int j = 0; j < items.Count; j++)
                {
                    if (itemManager.Inventory.Tally[items[j].item]
                        < items[j].count)
                    {
                        return;
                    }
                }
            }
            questNotifier.Notify();
        }

        public void OnDestroy()
        {
            ManagerServices.Instance.GetService<QuestSubscriptionHandler>().RemoveSubscriber(this);
        }

        public void Notify(Quest questToValidate)
        {
            for (int i = 0; i < questStates.Count; i++)
            {
                questStates[i].CalculateQuestConditions(questToValidate);
                if (questStates[i].GetIfAllConditionsAreMet())
                {
                    questStates[i].onComplete.Invoke();
                    questStates.RemoveAt(i);
                }
            }
        }

        public void OnValidate()
        {
            if (questStates == null)
            {
                return;
            }

            foreach (var questState in questStates)
            {
                questState.OnValidate();
            }
        }
    }
}