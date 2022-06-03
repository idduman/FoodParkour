using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    [RequireComponent(typeof(Rigidbody))]
    public class ConveyorBelt : MonoBehaviour
    {
        public bool Active;

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
        }

        private void OnDisable()
        {
            GameManager.Finished -= OnFinish;
        }

        private void OnFinish()
        {
            Active = false;
        }

        void FixedUpdate()
        {
            if (!Active)
                return;

            var pos = _rb.position;
            pos.z -= _currentMoveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(pos);
        }
    }

}
