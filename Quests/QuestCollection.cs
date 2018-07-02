using System.Collections.Generic;
using DownTheWell.Utility;
using UnityEngine;
using System;
using System.IO;


namespace DownTheWell.Gameplay.World.Quests
{
    public enum QuestStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Failed
    }

    [Serializable]
    public class Quest
    {
        public string title;
        public string description;
        public QuestStatus status;

        public override string ToString()
        {
            return title;
        }

        public override bool Equals(object obj)
        {
            var quest = obj as Quest;
            return quest != null &&
                   title == quest.title &&
                   description == quest.description;
        }

        public override int GetHashCode()
        {
            var hashCode = 982426329;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
            return hashCode;
        }

        public static bool operator ==(Quest obj1, Quest obj2)
        {
            bool o1Null = ReferenceEquals(obj1, null);
            bool o2Null = ReferenceEquals(obj2, null);

            if (o1Null && o2Null)
            {
                return true;
            }
            if (o1Null)
            {
                return false;
            }
            if (o2Null)
            {
                return false;
            }

            return obj1.description == obj2.description &&
                obj1.title == obj2.title;
        }

        public static bool operator !=(Quest obj1, Quest obj2)
        {
            return !(obj1 == obj2);
        }
    }

    [Serializable]
    public class QuestLine
    {
        public string questlineTitle;
        public string description;
        public QuestStatus status;
        public List<Quest> quests;
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/Quests/QuestCollection")]
    public class QuestCollection : SingletonScriptableObject<QuestCollection>
    {
        private static string fullPath = "Assets/ScriptableObjects/Quest/Resources/BaseQuestCollection.asset";

        public List<Quest> quests;
        public List<QuestLine> questlines;

        public static QuestCollection GetInstance()
        {
#if !UNITY_EDITOR
            string path = Path.GetFileNameWithoutExtension(fullPath);
            return GetInstance(path);
#else
            if (UnityEditor.EditorApplication.isPlaying)
            {
                string path = Path.GetFileNameWithoutExtension(fullPath);
                return GetInstance(path);
            }
            else
            {
                return GetInstance(fullPath);
            }
#endif
        }

        public void Copy(QuestCollection another)
        {
            quests = new List<Quest>();
            questlines = new List<QuestLine>();

            for (int i = 0; i < another.quests.Count; i++)
            {
                Quest newQuest = new Quest
                {
                    description = another.quests[i].description,
                    status = another.quests[i].status,
                    title = another.quests[i].title
                };

                quests.Add(newQuest);
            }
        }
    }
}

