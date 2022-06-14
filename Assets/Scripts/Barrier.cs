using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class Barrier : MonoBehaviour
    {
        public static event Action GateOpen;
        public bool IsOpen;
        
        [SerializeField] private Transform _armTransform;

        private float _duration;
        private Tweener _armTween;

        private void Start()
        {
            _duration = GameManager.Instance.Config.BarrierToggleDuration;
            IsOpen = true;
            _armTransform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }

        private void OnDestroy()
        {
            _armTween.Kill();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Table") || other.CompareTag("Car"))
            {
                ToggleGate(true);
            }
        }

        public void ToggleGate(bool open)
        {
            if (IsOpen == open)
                return;

            IsOpen = open;

            _armTween.Kill();
            _armTween = _armTransform.DOLocalRotate(new Vector3(0f, 0f, open ? 90f : 0f), _duration)
                .OnComplete(FireEvents);
        }

        private void FireEvents()
        {
            if(IsOpen)
                GateOpen?.Invoke();

        }
    }
}

