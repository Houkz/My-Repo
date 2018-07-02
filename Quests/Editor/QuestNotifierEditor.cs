using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;
using System;

namespace DownTheWell.Gameplay.World.Quests
{
    [CustomEditor(typeof(QuestNotifier))]
    public class QuestNotifierEditor : UnityEditor.Editor
    {
        private QuestNotifier questNotifier;
        private List<string> questTitles;
        private List<Quest> quests;

        private int currentAmountOfQuestsToChange;

        public void Awake()
        {
            questNotifier = (QuestNotifier)target;
            quests = QuestCollection.GetInstance().quests;

            if (questNotifier.targetSelections == null)
            {
                questNotifier.targetSelections = new List<Selection>();
                for (int i = 0; i < questNotifier.amountOfQuestsToChange; i++)
                {
                    Selection newSelection = new Selection
                    {
                        targetQuest = quests[0],
                        desiredStatus = quests[0].status
                    };
                    questNotifier.targetSelections.Add(newSelection);
                }
            }

            if (questTitles == null)
            {
                questTitles = new List<string>();
                for (int i = 0; i < quests.Count; i++)
                {
                    questTitles.Add(quests[i].title);
                }
            }

            currentAmountOfQuestsToChange = questNotifier.amountOfQuestsToChange;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (currentAmountOfQuestsToChange != questNotifier.amountOfQuestsToChange)
            {
                ResizeQuestAmount();
            }

            for (int i = 0; i < questNotifier.amountOfQuestsToChange; i++)
            {
                DisplayInspector(questNotifier.targetSelections[i]);
            }

            if (GUI.changed)
            {
                EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            }
        }

        private void ResizeQuestAmount()
        {
            int difference = questNotifier.amountOfQuestsToChange - questNotifier.targetSelections.Count;

            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    Selection newSelection = new Selection
                    {
                        targetQuest = quests[0],
                        desiredStatus = quests[0].status
                    };
                    questNotifier.targetSelections.Add(newSelection);
                }
            }
            else if (difference < 0)
            {
                for (int i = 0; i < Mathf.Abs(difference); i++)
                {
                    questNotifier.targetSelections.RemoveAt(questNotifier.targetSelections.Count - 1);
                }
            }

            currentAmountOfQuestsToChange = questNotifier.amountOfQuestsToChange;
        }

        private void DisplayInspector(Selection currentSelection)
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical("box");

            DisplayPopup(currentSelection);
            DisplayStatus(currentSelection);

            EditorGUILayout.EndVertical();
        }

        private void DisplayPopup(Selection currentSelection)
        {
            GUILayout.Label("Targeted Quest");

            int selectionIndex = quests.IndexOf(currentSelection.targetQuest);
            int popupSelection = EditorGUILayout.Popup(selectionIndex, questTitles.ToArray());

            bool alreadyExists = false;
            for (int i = 0; i < questNotifier.targetSelections.Count; i++)
            {
                int existingIndex = quests.IndexOf(questNotifier.targetSelections[i].targetQuest);
                if (existingIndex == popupSelection)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (popupSelection != selectionIndex && alreadyExists == false)
            {
                currentSelection.targetQuest = quests[popupSelection];
            }
        }

        private void DisplayStatus(Selection currentSelection)
        {
            GUILayout.Label("Desired Status Of Quest");
            string[] enumNames = Enum.GetNames(typeof(QuestStatus));

            int currentStatus = GUILayout.SelectionGrid((int)currentSelection.desiredStatus, enumNames, 1);
            if (currentStatus != (int)currentSelection.desiredStatus)
            {
                currentSelection.desiredStatus = (QuestStatus)currentStatus;
            }
        }
    }
}