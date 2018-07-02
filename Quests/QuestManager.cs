using System.Collections.Generic;
using UnityEngine;
using DownTheWell.Utility.ServiceLocator;

namespace DownTheWell.Gameplay.World.Quests
{
    public class QuestManager : Service<ManagerServices>
    {
        private QuestCollection activeQuestCollection;
        protected override void Awake()
        {
            base.Awake();
            QuestCollection collection = QuestCollection.GetInstance();
            activeQuestCollection = ScriptableObject.CreateInstance<QuestCollection>();
            activeQuestCollection.Copy(collection);
        }

        public void UpdateQuestCollection(List<Selection> targetSelections)
        {
            List<Quest> quests = activeQuestCollection.quests;
            for (int i = 0; i < targetSelections.Count; i++)
            {
                for (int j = 0; j < quests.Count; j++)
                {
                    if (quests[j] == targetSelections[i].targetQuest)
                    {
                        quests[j].status = targetSelections[i].desiredStatus;
                        ManagerServices.Instance.GetService<QuestSubscriptionHandler>().NotifySubscribers(quests[j]);
                        break;
                    }
                }
            }
        }

        public QuestCollection GetActiveQuestCollection()
        {
            return activeQuestCollection;
        }

        public QuestStatus? GetQuestStatus(string questName)
        {
            foreach (Quest quest in activeQuestCollection.quests)
            {
                if (quest.title.Equals(questName))
                {
                    return quest.status;
                }
            }

            foreach (QuestLine questLine in activeQuestCollection.questlines)
            {
                foreach (Quest quest in questLine.quests)
                {
                    if (quest.title.Equals(questName))
                    {
                        return quest.status;
                    }
                }
            }

            Debug.LogWarning("[QuestManager] Didn't find quest named '" + questName + "'");
            return null;
        }

        public void SetQuestStatus(string questName, QuestStatus questStatus)
        {
            foreach (Quest quest in activeQuestCollection.quests)
            {
                if (quest.title.Equals(questName))
                {
                    quest.status = questStatus;
                    ManagerServices.Instance.GetService<QuestSubscriptionHandler>().NotifySubscribers(quest);
                    return;
                }
            }

            foreach (QuestLine questLine in activeQuestCollection.questlines)
            {
                foreach (Quest quest in questLine.quests)
                {
                    if (quest.title.Equals(questName))
                    {
                        quest.status = questStatus;
                        ManagerServices.Instance.GetService<QuestSubscriptionHandler>().NotifySubscribers(quest);
                        return;
                    }
                }
            }

            Debug.LogWarning("[QuestManager] Didn't find quest named '" + questName + "'");
        }

        public QuestStatus? GetQuestLineStatus(string questLineName)
        {
            foreach (QuestLine questLine in activeQuestCollection.questlines)
            {
                if (questLine.questlineTitle.Equals(questLineName))
                {
                    return questLine.status;
                }
            }

            Debug.LogWarning("[QuestManager] Didn't find questLine named '" + questLineName + "'");
            return null;
        }

        public void SetQuestLineStatus(string questLineName, QuestStatus questStatus)
        {
            foreach (QuestLine questLine in activeQuestCollection.questlines)
            {
                if (questLine.questlineTitle.Equals(questLineName))
                {
                    questLine.status = questStatus;
                    return;
                }
            }

            Debug.LogWarning("[QuestManager] Didn't find questLine named '" + questLineName + "'");
        }
    }
}