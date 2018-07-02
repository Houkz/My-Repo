using System;
using UnityEngine;

namespace Core.InputSystem
{
    [Serializable]
    public class InputSetState
    {
        [SerializeField]
        public BaseInputSet inputSet;
        [SerializeField]
        public bool shouldBeActive;
    }
}