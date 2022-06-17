using System;
using System.Collections;
using HyperCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dixy.FoodParkour
{
    [RequireComponent(typeof(Rigidbody))]
    public class Food : MonoBehaviour
    {
        public static event Action<float> FoodEaten;
        
        [SerializeField] private float _points = 1f;
        [SerializeField] private bool _sticky = false;
        [SerializeField] private float _stickDuration = 1f;
        [SerializeField] private float _stickVariance = 0.15f;
        [SerializeField] private bool _stickToShirt = false;

        private int _fanLayer;
        private int _floorLayer;
        private int _noclipLayer;
        private int _playerLayer;
        private int _beltLayer;

        private float _stickyTimer;
        
        private Rigidbody _rb;

        private bool _thrown;
        private bool _eaten;
        private bool _sticked;

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
            if (other.gameObject.layer == _beltLayer)
            {
                _eaten = true;
                gameObject.layer = _noclipLayer;
                if(_rb)
                    _rb.isKinematic = true;
                transform.parent = other.transform;
            }
            else if (other.gameObject.layer == _floorLayer)
            {
                _eaten = true;
                Destroy(gameObject);
                return;
            }
            else if (_stickToShirt && !_sticked && other.collider.CompareTag("TShirt"))
            {
                _sticked = true;
                _rb.isKinematic = true;
                gameObject.layer = _noclipLayer;
            }

            if (Eaten || _sticked)
                return;

            if (other.gameObject.layer == _playerLayer)
            {
                _eaten = true;
                _thrown = true;
                transform.parent = GameManager.Instance.Level.transform;
                if (_sticky)
                    Stick();
                else
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

        public void Stick()
        {
            if (_rb.isKinematic)
                return;

            _rb.isKinematic = true;
            StartCoroutine(StickRoutine());
        }

        public void OnEaten()
        {
            _eaten = true;
            gameObject.layer = _noclipLayer;
            if(_rb)
                _rb.isKinematic = true;
            
            FoodEaten?.Invoke(_points);
        }

        private IEnumerator StickRoutine()
        {
            yield return new WaitForSeconds(_stickDuration + Random.Range(-_stickVariance, _stickVariance));
            _rb.isKinematic = false;
        }
    }
}

