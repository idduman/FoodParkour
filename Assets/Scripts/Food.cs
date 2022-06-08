using System;
using System.Security.Cryptography;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    [RequireComponent(typeof(Rigidbody))]
    public class Food : MonoBehaviour
    {
        private int _fanLayer;
        private int _floorLayer;
        private int _noclipLayer;
        private int _playerLayer;
        private Rigidbody _rb;

        private bool _thrown;

        private void Start()
        {
            _fanLayer = LayerMask.NameToLayer("Fan");
            _floorLayer = LayerMask.NameToLayer("Floor");
            _noclipLayer = LayerMask.NameToLayer("Food_Noclip");
            _playerLayer = LayerMask.NameToLayer("Player");
            _rb = GetComponent<Rigidbody>();
            _thrown = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _fanLayer && !_thrown)
            {
                _thrown = true;
                transform.parent = GameManager.Instance.Level.transform;
                _rb.isKinematic = false;
                _rb.constraints = RigidbodyConstraints.None;
                _rb.AddForce(GameManager.Instance.Config.FoodFlyVector.normalized * GameManager.Instance.Config.FoodFlySpeed, ForceMode.VelocityChange);
                _rb.AddTorque(Vector3.forward, ForceMode.VelocityChange);
                return;
            }
            if (other.gameObject.CompareTag("Mouth") && _thrown)
            {
                gameObject.layer = _noclipLayer;
                /*transform.parent = other.transform;
                _rb.velocity = Vector3.zero;
                _rb.isKinematic = true;*/
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == _floorLayer)
            {
                Destroy(gameObject);
                return;
            }
            if (other.gameObject.layer == _playerLayer && !_thrown)
            {
                _thrown = true;
                transform.parent = GameManager.Instance.Level.transform;
                _rb.isKinematic = false;
            }
        }
    }
}

