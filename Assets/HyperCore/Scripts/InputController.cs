using System;
using UnityEngine;

namespace HyperCore
{
    public class InputController : SingletonBehaviour<InputController>
    {
        public event Action<Vector3> Pressed;
        public event Action<Vector3> Moved;
        public event Action<Vector3> Released;

        private const float _threshold = 0f;

        private Vector3 _lastInput;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastInput = Input.mousePosition;
                Pressed?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Released?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                var inputDelta = (Input.mousePosition - _lastInput) / Screen.width;
                if (inputDelta.magnitude >= _threshold)
                {
                    _lastInput = Input.mousePosition;
                    Moved?.Invoke(inputDelta);
                }
            }
        }
    }
}
