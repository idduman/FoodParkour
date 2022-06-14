using System;
using System.Security.Cryptography;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    [RequireComponent(typeof(Rigidbody))]
    public class Food : MonoBehaviour
    {
        public static event Action<float> FoodEaten;
        
        [SerializeField] private float _points = 1f;
    
        private int _fanLayer;
        private int _floorLayer;
        private int _noclipLayer;
        private int _playerLayer;
        private int _beltLayer;
        private Rigidbody _rb;

        private bool _thrown;
        private bool _eaten;

        public bool Eaten => _eaten;

        private void Start()
        {
            _fanLayer = LayerMask.NameToLayer("Fan");
            _floorLayer = LayerMask.NameToLayer("Floor");
            _beltLayer = LayerMask.NameToLayer("Belt");
            _noclipLayer = LayerMask.NameToLayer("Food_Noclip");
            _playerLayer = LayerMask.NameToLayer("Player");
            _rb = GetComponent<Rigidbody>();
            _thrown = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Eaten)
                return;
            if (other.gameObject.layer == _fanLayer)
            {
                Throw(GameManager.Instance.Config.FoodFlyVector.normalized * GameManager.Instance.Config.FoodFlySpeed, true);
                return;
            }
            if (other.gameObject.CompareTag("ReleaseFood"))
            {
                transform.parent = GameManager.Instance.Level.transform;
                _rb.isKinematic = false;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (Eaten)
                return;

            if (other.gameObject.layer == _beltLayer)
            {
                OnEaten();
                transform.parent = other.transform;
            }
            else if (other.gameObject.layer == _floorLayer)
            {
                _eaten = true;
                Destroy(gameObject);
                return;
            }
            else if (other.gameObject.layer == _playerLayer && !_thrown)
            {
                _thrown = true;
                transform.parent = GameManager.Instance.Level.transform;
                _rb.isKinematic = false;
            }
        }

        public void Throw(Vector3 velocity, bool addTorque = false)
        {
            if (Eaten || _thrown)
                return;
            
            _thrown = true;
            
            transform.parent = GameManager.Instance.Level.transform;
            _rb.isKinematic = false;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.AddForce(velocity, ForceMode.VelocityChange);
            if(addTorque)
                _rb.AddTorque(Vector3.forward, ForceMode.VelocityChange);
        }

        public void OnEaten()
        {
            _eaten = true;
            gameObject.layer = _noclipLayer;
            if(_rb)
                _rb.isKinematic = true;
            
            FoodEaten?.Invoke(_points);
        }
    }
}

