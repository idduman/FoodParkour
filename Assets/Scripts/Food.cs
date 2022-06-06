using System;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    [RequireComponent(typeof(Rigidbody))]
    public class Food : MonoBehaviour
    {
        private int _fanLayer;
        private Rigidbody _rb;

        private bool _thrown;
        private readonly Vector3 throwVector = new Vector3(7, 1, 0).normalized;

        private void Start()
        {
            _fanLayer = LayerMask.NameToLayer("Fan");
            //_beltLayer = LayerMask.NameToLayer("Belt");
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
                _rb.AddForce(throwVector * 20f, ForceMode.VelocityChange);
                return;
            }

            if (other.gameObject.CompareTag("Player") && _thrown)
            {
                
            }
        }
    }
}

