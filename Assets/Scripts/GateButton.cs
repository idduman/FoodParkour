using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class GateButton : MonoBehaviour
    {
        public event Action<Material> Pressed;

        public bool IsPressed
        {
            get;
            private set;
        }

        private Material _sharedMaterial;
        void Start()
        {
            _sharedMaterial = GetComponent<Renderer>().sharedMaterial;
            IsPressed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsPressed && other.CompareTag("LunchBox"))
            {
                transform.DOMoveY(transform.position.y - 0.2f, 0.2f);
                IsPressed = true;
                Pressed?.Invoke(_sharedMaterial);
            }
        }
    }
}

