using DownTheWell.Utility.ServiceLocator;
using DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates;
using System.Collections.Generic;
using UnityEngine;


namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests
{
    public class QuestEventHandler : MonoBehaviour, IQuestSubscriber
    {
        [SerializeField]
        public List<UnityEventQuestState> questStates;
        private UnityEventQuestState currentQuestState;

        public void Start()
        {
            ManagerServices.Instance.GetService<QuestSubscriptionHandler>().AddSubscriber(this);

            List<Quest> questList = QuestCollection.GetInstance().quests;
            foreach (Quest quest in questList)
            {
                Notify(quest);
            }
        }

        public void OnDestroy()
        {
            ManagerServices.Instance.GetService<QuestSubscriptionHandler>().RemoveSubscriber(this);
        }

        public void Notify(Quest questToValidate)
        {
            UnityEventQuestState oldNewCurrentState = currentQuestState;
            foreach (var state in questStates)
            {
                state.CalculateQuestConditions(questToValidate);
                if (state.GetIfAllConditionsAreMet() == true)
                {
                    currentQuestState = state;
                }
                else
                {
                    if (state == currentQuestState)
                    {
                        Debug.Log("No conditions are true.");
                    }
                }
            }

            if (oldNewCurrentState != currentQuestState)
            {
                TriggerState();
            }
        }

        public void OnValidate()
        {
            foreach (var state in questStates)
            {
                state.OnValidate();
            }
        }

        private void TriggerState()
        {
            currentQuestState.unityEventToTrigger.Invoke();
        }

        public void DebugUnityEvent(string someText)
        {
            Debug.Log(someText);
        }
    }
}
