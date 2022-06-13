using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class CornTable : MonoBehaviour
    {
        [SerializeField] private Transform _cornArmature;
        //[SerializeField] private Transform _cornTransform;
        
        private int _machineLayer;
        private int _barrierLayer;
        private bool _triggered;
        
        private void Start()
        {
            _machineLayer = LayerMask.NameToLayer("Machine");
            _barrierLayer = LayerMask.NameToLayer("Barrier");
        }

        private void OnDestroy()
        {
            _cornArmature.DOKill();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered && other.gameObject.layer == _machineLayer)
            {
                _triggered = true;
                _cornArmature.DOKill();
                _cornArmature.DORotate(360 * Vector3.right, 1f,
                        RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1);
                    
                /*if(!_cornTransform)
                    return;
                    
                _cornTransform.DOKill();
                _cornTransform.DORotate(360 * Vector3.forward, 1f,
                        RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1);*/
                    
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (_triggered && other.gameObject.layer == _machineLayer)
            {
                _triggered = false;
                _cornArmature.DOKill();
                /*if (_cornTransform)
                    _cornTransform.DOKill();*/
            }
        }
    }

}
