using UnityEngine;

namespace DownTheWell.Gameplay.World.Quests
{
    [RequireComponent(typeof(QuestNotifier))]
    public class QuestNotificationHandler : MonoBehaviour
    {
        private QuestNotifier questNotifier;

        protected virtual void Start()
        {
            questNotifier = GetComponent<QuestNotifier>();
        }

        protected virtual void Notify()
        {
            questNotifier.Notify();
        }
    }
}
