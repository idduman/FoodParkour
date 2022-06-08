using System;
using DG.Tweening;
using HyperCore;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class SoupTable : MonoBehaviour
    {
        [SerializeField] private Transform _armature;

        private Vector3 _armatureRotation;
        private Vector3 _handleRotation;
        private int _machineLayer;
        private bool _triggered;
        private Sequence _fallSequence;
        
        private void Start()
        {
            _armatureRotation = _armature.localRotation.eulerAngles;
            _machineLayer = LayerMask.NameToLayer("Machine");
        }
        private void OnDestroy()
        {
            _fallSequence.Kill();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered && other.gameObject.layer == _machineLayer)
            {
                var fDuration = GameManager.Instance.Config.SoupFallDuration;
                var tDuration = GameManager.Instance.Config.SoupTurnDuration;
                _triggered = true;
                
                _fallSequence.Kill();
                _fallSequence = DOTween.Sequence();
                
                _fallSequence.Append(
                    _armature.DOLocalRotate(new Vector3(_armatureRotation.x - 25f, _armatureRotation.y, _armatureRotation.z),
                        fDuration)).SetEase(Ease.OutQuad);
                
                _fallSequence.Append(
                    _armature.DOLocalRotate(new Vector3(_armatureRotation.x - 45f, _armatureRotation.y, _armatureRotation.z),
                        tDuration)).SetEase(Ease.Linear);
                
                _fallSequence.Play();
            }
        }

    }
}


