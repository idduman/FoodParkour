using System;
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
                transform.parent = other.transform.parent;
                _rb.isKinematic = false;
                _rb.constraints = RigidbodyConstraints.None;
                _rb.AddForce(throwVector * 5f, ForceMode.VelocityChange);
            }
        }
    }
}

