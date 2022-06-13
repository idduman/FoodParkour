using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class TrainMachine : MonoBehaviour
    {
        [SerializeField] private Transform _trainArmature;
        [SerializeField] private Transform _trainEnd;
        
        private int _tableLayer;
        private bool _triggered;
        
        private void Start()
        {
            _tableLayer = LayerMask.NameToLayer("Table");
        }

        private void OnDestroy()
        {
            _trainArmature.DOKill();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered && other.gameObject.CompareTag("Car"))
            {
                _triggered = true;
                _trainArmature.DOKill();
                _trainArmature.DOLocalMove(_trainEnd.localPosition, GameManager.Instance.Config.TrainMoveDuration)
                    .SetEase(Ease.Linear);
            }
        }
        
        /*private void OnTriggerExit(Collider other)
        {
            if (_triggered && other.gameObject.layer == _machineLayer)
            {
                _triggered = false;
                _trainArmature.DOKill();
            }
        }*/
    }

}

