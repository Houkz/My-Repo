using System;
using InControl;
using UnityEngine;

namespace Core.InputSystem
{
    public abstract class BaseInputSet<T> : BaseInputSet
        where T : PlayerActionSet
    {
        public T actionSet;

        private void Awake()
        {
            RaiseOnCreated(this);
        }

        public override void SetEnabled(bool value)
        {
            actionSet.Enabled = value;
        }
    }

    public abstract class BaseInputSet : ScriptableObject
    {
        public abstract void InitializeActionSet();
        public abstract void SetEnabled(bool value);

        public static void RaiseOnCreated(BaseInputSet inputSet)
        {
            if (OnCreated != null)
            {
                OnCreated(inputSet);
            }
        }

        // TODO: Should not exist in runtime (only #if UNITY_EDITOR)
        public static event Action<BaseInputSet> OnCreated;
    }
}
