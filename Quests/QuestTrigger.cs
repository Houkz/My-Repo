using DownTheWell.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DownTheWell.Gameplay.World.Quests
{

    public class QuestTrigger : QuestNotificationHandler
    {
        [SerializeField]
        private bool triggerOnlyOnce;
        private bool hasBeenTriggered;

        protected override void Start()
        {
            base.Start();
            hasBeenTriggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerLayer = PlayerCharacterController.Instance.gameObject.layer;
            if (other.gameObject.layer == playerLayer)
            {
                if (!triggerOnlyOnce)
                {
                    Notify();
                    return;
                }

                if (!hasBeenTriggered)
                {
                    hasBeenTriggered = true;
                    Notify();
                }
            }
        }
    }
}
