using System.Collections.Generic;
using UnityEngine;
using DownTheWell.Utility.ServiceLocator;
using System;

namespace DownTheWell.Gameplay.World.Quests
{
    [Serializable]
    public class Selection
    {
        public Quest targetQuest;
        public QuestStatus desiredStatus;
    }

    public class QuestNotifier : MonoBehaviour
    {
        private QuestManager questManager;
        public int amountOfQuestsToChange;

        [HideInInspector]
        [SerializeField]
        public List<Selection> targetSelections;

        public void Start()
        {
            questManager = ManagerServices.Instance.GetService<QuestManager>();
        }

        public void Notify()
        {
            questManager.UpdateQuestCollection(targetSelections);
        }
    }

}
