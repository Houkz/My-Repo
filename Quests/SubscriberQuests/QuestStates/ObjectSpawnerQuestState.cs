using System.Collections.Generic;
using UnityEngine;
using System;

namespace DownTheWell.Gameplay.World.Quests.SubscriberQuests.QuestStates
{
    [Serializable]
    public struct ObjectToSpawn
    {
        [SerializeField]
        public GameObject objectPrefab;
        [SerializeField]
        public Transform objectTransform;
    }

    [Serializable]
    public class ObjectSpawnerQuestState : QuestState
    {
        [SerializeField]
        public List<ObjectToSpawn> objectsToSpawn = new List<ObjectToSpawn>();
    }
}