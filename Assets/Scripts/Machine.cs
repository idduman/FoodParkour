using System;
using System.Collections;
using System.Collections.Generic;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class Machine : MonoBehaviour
    {
        [SerializeField] private bool _lowersBarrier = true;
        private Transform _conveyorTransform;

        private void Awake()
        {
            _conveyorTransform = transform.parent;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_lowersBarrier && other.TryGetComponent<Barrier>(out var barrier))
            {
                barrier.ToggleGate(false);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var col = other.collider.transform;
            if (col.CompareTag("Arm"))
            {
                transform.parent = col.parent.parent;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            var col = other.collider.transform;
            if (col.CompareTag("Arm"))
            {
                transform.parent = _conveyorTransform;
            }
        }
    }
}

