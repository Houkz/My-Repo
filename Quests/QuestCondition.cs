using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace DownTheWell.Gameplay.World.Quests
{
    [Serializable]
    public class QuestCondition
    {
        [SerializeField, HideInInspector]
        private Quest serializedQuest;
        [SerializeField, HideInInspector]
        private bool hasConditionBeenMet;

        [ValueDropdown("Quests")]
        [SerializeField]
        public string questString;
        [SerializeField]
        public QuestStatus status;

        public void Initialize()
        {
            hasConditionBeenMet = false;
        }

        private List<string> Quests
        {
            get
            {
                var quests = QuestCollection.GetInstance().quests;
                List<string> questStrings = new List<string>(quests.Count);
                foreach (var quest in quests)
                {
                    questStrings.Add(quest.title);
                }
                return questStrings;
            }
        }

        public void OnValidate()
        {
            foreach (var quest in QuestCollection.GetInstance().quests)
            {
                if (quest.title == questString)
                {
                    serializedQuest = quest;
                    break;
                }
            }
        }

        public bool EvaluateCondition()
        {
            return hasConditionBeenMet;
        }

        public void ValidateCondition(Quest someQuest)
        {
            if (serializedQuest == someQuest)
            {
                if (someQuest.status == status)
                {
                    hasConditionBeenMet = true;
                }
                else
                {
                    hasConditionBeenMet = false;
                }
            }
        }
    }
}