using DownTheWell.Utility.ServiceLocator;
using DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates;
using System.Collections.Generic;
using UnityEngine;
using DownTheWell.Gameplay.UI.Dialogue;
using System;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests
{
    [Serializable]
    public class ObjectDialogueQuestState : QuestState
    {
        [SerializeField]
        public TextAsset dialogue;
    }

    [RequireComponent(typeof(BasicDialogue))]
    public class QuestDialoguePicker : MonoBehaviour, IQuestSubscriber
    {
        [SerializeField]
        public List<ObjectDialogueQuestState> questStates;
        private ObjectDialogueQuestState currentQuestState;
        private BasicDialogue basicDialogue;

        private void Awake()
        {
            basicDialogue = GetComponent<BasicDialogue>();
        }

        public void Start()
        {
            ManagerServices.Instance.GetService<QuestSubscriptionHandler>().AddSubscriber(this);

            List<Quest> questList = ManagerServices.Instance.GetService<QuestManager>().GetActiveQuestCollection().quests;
            foreach (Quest quest in questList)
            {
                Notify(quest);
            }
        }

        public void Notify(Quest questToValidate)
        {
            ObjectDialogueQuestState oldNewCurrentState = currentQuestState;
            foreach (var state in questStates)
            {
                state.CalculateQuestConditions(questToValidate);
                if (state.GetIfAllConditionsAreMet() == true)
                {
                    currentQuestState = state;
                }
            }

            if (oldNewCurrentState != currentQuestState)
            {
                UpdateActiveDialogue();
            }
        }

        public void OnDestroy()
        {
            ManagerServices.Instance.GetService<QuestSubscriptionHandler>().RemoveSubscriber(this);
        }

        private void UpdateActiveDialogue()
        {
            basicDialogue.InkStory = currentQuestState.dialogue;
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