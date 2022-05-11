using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class GateButton : MonoBehaviour
    {
        public event Action<Material> Pressed;

        private Material _sharedMaterial;
        void Start()
        {
            _sharedMaterial = GetComponent<Renderer>().sharedMaterial;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LunchBox"))
            {
                transform.DOMoveY(transform.position.y - 0.2f, 0.2f);
                Pressed?.Invoke(_sharedMaterial);
            }
        }
    }
}

