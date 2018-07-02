using UnityEngine;
using UnityEngine.Events;
using System;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates
{
    [Serializable]
    public class UnityEventQuestState : QuestState
    {
        [SerializeField]
        public UnityEvent unityEventToTrigger;
    }
}
