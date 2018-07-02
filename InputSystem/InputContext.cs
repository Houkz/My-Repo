using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Core.InputSystem
{
    [CreateAssetMenu(fileName = "InputContext", menuName = "Forgotten Key/InputSystem/InputContext")]
    public class InputContext : ScriptableObject
    {
        [SerializeField]
        public List<InputSetState> inputSetStates;

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (inputSetStates == null)
            {
                Init();
            }
            BaseInputSet.OnCreated += OnInputSetCreated;
#endif
        }

#if UNITY_EDITOR
        private void Init()
        {
            inputSetStates = new List<InputSetState>();

            string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Format("t:BaseInputSet", typeof(BaseInputSet)));
            for (int i = 0; i < guids.Length; i++)
            {
                LoadAsset(guids[i]);
            }
        }

        private void LoadAsset(string guid)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            BaseInputSet asset = UnityEditor.AssetDatabase.LoadAssetAtPath<BaseInputSet>(assetPath);
            if (asset != null)
            {
                OnInputSetCreated(asset);
            }
        }

        private void OnDisable()
        {
            BaseInputSet.OnCreated -= OnInputSetCreated;
        }
#endif

        private void OnInputSetCreated(BaseInputSet newInputSet)
        {
            if (ExistsInInputSetStates(newInputSet) == false)
            {
                InputSetState newState = new InputSetState
                {
                    inputSet = newInputSet
                };
                inputSetStates.Add(newState);
            }
        }

        private bool ExistsInInputSetStates(BaseInputSet inputSet)
        {
            for (int i = 0; i < inputSetStates.Count; i++)
            {
                if (inputSet.GetType() == inputSetStates[i].inputSet.GetType())
                {
                    Debug.LogWarning("Attempted to add an already existing InputSet to the InputContext. This is not allowed!");
                    return true;
                }
            }
            return false;
        }
    }
}
