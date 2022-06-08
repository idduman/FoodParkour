using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    [RequireComponent(typeof(Rigidbody))]
    public class ConveyorBelt : MonoBehaviour
    {
        public bool Active;
        private bool _started;

        private Rigidbody _rb;

        private float _currentMoveSpeed;
        void Start()
        {
            Active = true;
            _currentMoveSpeed = GameManager.Instance.Config.ConveyorMoveSpeed;
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            GameManager.Finished += OnFinish;
            InputController.Instance.Pressed += OnPressed;
        }

        private void OnDisable()
        {
            GameManager.Finished -= OnFinish;
            if(InputController.Instance)
                InputController.Instance.Pressed -= OnPressed;
        }
        
        private void OnPressed(Vector3 obj)
        {
            _started = true;
        }

        private void OnFinish()
        {
            Active = false;
            InputController.Instance.Pressed -= OnPressed;
        }

        void FixedUpdate()
        {
            if (!_started ||  !Active)
                return;

            var pos = _rb.position;
            pos.z -= _currentMoveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(pos);
        }
    }

}
