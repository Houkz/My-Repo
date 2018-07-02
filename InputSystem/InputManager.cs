using System.Collections.Generic;
using UnityEngine;
using ForgottenKey.Lenses;
using System;
using ForgottenKey;

namespace Core.InputSystem
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField]
        private InputContext defaultInputContext;
        
        private LensedValue<InputContext> currentInputContext;
        private Dictionary<Type, BaseInputSet> allInputSets;
        private int lastContextId;

        protected override void Awake()
        {
            base.Awake();
            currentInputContext = new LensedValue<InputContext>(defaultInputContext);
            lastContextId = currentInputContext.Value.GetInstanceID();

            CacheActionSets();
            InitializeActionSets();

            UpdateCurrentState(defaultInputContext);
        }

        private void CacheActionSets()
        {
            allInputSets = new Dictionary<Type, BaseInputSet>();
            foreach (InputSetState state in defaultInputContext.inputSetStates)
            {
                Type type = state.inputSet.GetType();
                allInputSets.Add(type, state.inputSet);
            }
        }

        private void InitializeActionSets()
        {
            foreach (InputSetState state in defaultInputContext.inputSetStates)
            {
                state.inputSet.InitializeActionSet();
            }
        }

        private void Update()
        {
            InputContext inputContext = currentInputContext.Value;

            int currentContextId = inputContext.GetInstanceID();
            if (currentContextId != lastContextId)
            {
                UpdateCurrentState(inputContext);
            }

            lastContextId = currentContextId;
        }

        private void UpdateCurrentState(InputContext inputContext)
        {
            foreach (InputSetState state in inputContext.inputSetStates)
            {
                state.inputSet.SetEnabled(state.shouldBeActive);
            }
        }

        public LensToken ActivateContext(InputContext inputContext)
        {
            LensToken token = currentInputContext.AddLens(new Lens<InputContext>(context =>
            {
                return inputContext;
            }));

            return token;
        }

        public BaseInputSet GetInputSet(Type inputSetType)
        {
            if (allInputSets[inputSetType] != null)
            {
                return allInputSets[inputSetType];
            }
            Debug.LogError("Could not find requested InputSet!");
            return null;
        }

        public T GetInputSet<T>() where T : BaseInputSet
        {
            if (allInputSets[typeof(T)] != null)
            {
                return (T)allInputSets[typeof(T)];
            }
            Debug.LogError("Could not find requested InputSet!");
            return null;
        }
    }
}
