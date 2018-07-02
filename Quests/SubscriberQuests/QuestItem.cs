using DownTheWell.Gameplay.UI.ItemSystem;
using UnityEngine;
using System;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests
{
    [RequireComponent(typeof(QuestNotifier))]
    public class QuestItem : DroppedItem
    {
        private QuestNotifier questNotifier;

        protected override void Start()
        {
            base.Start();
            questNotifier = GetComponent<QuestNotifier>();
        }

        public override void OnTapInput(Action action)
        {
            base.OnTapInput(action);
            questNotifier.Notify();
        }
    }
}
