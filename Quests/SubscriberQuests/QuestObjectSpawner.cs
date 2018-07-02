using DownTheWell.Utility.ServiceLocator;
using DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates;
using System.Collections.Generic;
using UnityEngine;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests
{
    public class QuestObjectSpawner : MonoBehaviour, IQuestSubscriber
    {
        [SerializeField]
        public List<ObjectSpawnerQuestState> questStates;
        private ObjectSpawnerQuestState currentQuestState;
        private List<GameObject> currentGameObjects;

        private void Awake()
        {
            currentGameObjects = new List<GameObject>();
        }

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
            ObjectSpawnerQuestState oldNewCurrentState = currentQuestState;
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
                        if (currentGameObjects.Count > 0)
                        {
                            RemoveSpawnedObjects();
                        }
                    }
                }
            }

            if (oldNewCurrentState != currentQuestState)
            {
                UpdateObjectsToSpawn();
            }
        }


        private void UpdateObjectsToSpawn()
        {
            List<ObjectToSpawn> objectsToSpawn = currentQuestState.objectsToSpawn;
            foreach (var objectToSpawn in objectsToSpawn)
            {
                if (objectToSpawn.objectPrefab == null)
                {
                    Debug.LogError("Object attempted to spawn in the QuestObjectSpawner is null. Add object prefab to the QuestObjectSpawner! {QuestObjectSpawner name: " + this + "}");
                    return;
                }

                GameObject newGameObject = Instantiate(objectToSpawn.objectPrefab);
                newGameObject.transform.SetPositionAndRotation(objectToSpawn.objectTransform.position, objectToSpawn.objectTransform.rotation);
                currentGameObjects.Add(newGameObject);
            }
        }

        private void RemoveSpawnedObjects()
        {
            for (int i = 0; i < currentGameObjects.Count;)
            {
                Destroy(currentGameObjects[i]);
                currentGameObjects.Remove(currentGameObjects[i]);
            }
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