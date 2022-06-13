using System.Collections;
using DG.Tweening;
using HyperCore;
using Obi;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class SoupTable : MonoBehaviour
    {
        [SerializeField] private Transform _armature;
        [SerializeField] private ObiEmitter _obiEmitter;

        private Vector3 _armatureRotation;
        private Vector3 _handleRotation;
        private int _machineLayer;
        private bool _triggered;
        private Sequence _fallSequence;
        
        private void Start()
        {
            _armatureRotation = _armature.localRotation.eulerAngles;
            _machineLayer = LayerMask.NameToLayer("Machine");
            _obiEmitter.enabled = false;
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
                        fDuration))
                    .SetEase(Ease.OutQuad)
                    .OnComplete(ActivateLiquid);
                
                _fallSequence.Append(
                    _armature.DOLocalRotate(new Vector3(_armatureRotation.x - 45f, _armatureRotation.y, _armatureRotation.z),
                        tDuration)).SetEase(Ease.Linear);
                
                _fallSequence.Play();
            }
        }

        private void ActivateLiquid()
        {
            if(!_obiEmitter.enabled)
                StartCoroutine(LiquidRoutine());
        }

        private IEnumerator LiquidRoutine()
        {
            _obiEmitter.enabled = true;
            yield return new WaitForSeconds(3);
            _obiEmitter.enabled = false;
        }

    }
}


