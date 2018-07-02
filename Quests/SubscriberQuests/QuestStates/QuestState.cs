using System.Collections.Generic;
using UnityEngine;
using System;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates
{
    [Serializable]
    public class QuestState
    {
        [SerializeField]
        private List<QuestCondition> questConditions;

        private void Awake()
        {
            foreach (QuestCondition condition in questConditions)
            {
                condition.Initialize();
            }
        }

        public void OnValidate()
        {
            if (questConditions == null)
            {
                return;
            }

            foreach (var condition in questConditions)
            {
                condition.OnValidate();
            }
        }

        public void CalculateQuestConditions(Quest someQuestToValidate)
        {
            foreach (QuestCondition condition in questConditions)
            {
                condition.ValidateCondition(someQuestToValidate);
            }
        }

        public bool GetIfAllConditionsAreMet()
        {
            foreach (QuestCondition condition in questConditions)
            {
                if (condition.EvaluateCondition() == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
