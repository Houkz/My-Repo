using System.Collections.Generic;
using UnityEngine;
using System;
using DownTheWell.Gameplay.UI.ItemSystem;
using UnityEngine.Events;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates
{
    [Serializable]
    public struct ItemToCheck
    {
        [SerializeField]
        public Item item;
        [SerializeField]
        public int count;
    }

    [Serializable]
    public class ItemCheckerQuestState : QuestState
    {
        [SerializeField]
        public List<ItemToCheck> itemsToCheck = new List<ItemToCheck>();
        [SerializeField]
        public UnityEvent onComplete;
    }
}