using UnityEngine;
using UnityEditor;

namespace Core.InputSystem.Editor
{
    [CustomPropertyDrawer(typeof(BaseInputSet), true)]
    public class InputSetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.serializedObject.targetObject is InputContext)
            {
                EditorGUI.PropertyField(position, property);
            }
            else
            {
                EditorGUI.LabelField(position, "n");
                Debug.LogWarning("You are trying to use direct reference to an inputset and this isn't allowed! Use InputManager instead!");
            }
            EditorGUI.EndProperty();
        }
    }
}

