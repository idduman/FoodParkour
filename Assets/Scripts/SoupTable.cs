using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private ObiSolver _obiSolver;
        [SerializeField] private GameObject _painterPrefab;

        private Vector3 _armatureRotation;
        private Vector3 _handleRotation;
        private int _machineLayer;
        private bool _triggered;
        private Sequence _fallSequence;
        private List<Transform> _painters = new List<Transform>();
        private Transform _particlePool;

        private int _particleCounter;
        
        private void Start()
        {
            _armatureRotation = _armature.localRotation.eulerAngles;
            _machineLayer = LayerMask.NameToLayer("Machine");
            _obiEmitter.enabled = false;
            
            _particlePool = transform.Find("Pool");
            for (int i = 0; i < GameManager.Instance.Config.SoupPainterParticleAmount; i++)
            {
                _painters.Add(Instantiate(_painterPrefab, _particlePool).transform);
            }
        }
        
        private void Update()
        {
            if (!_obiEmitter.enabled)
                return;

            for (int i = 0; i < _painters.Count; i++)
            {
                var index = (i * 5) % _particleCounter;
                _painters[i].position = _obiEmitter.GetParticlePosition(index);
            }
        }

        private void OnDestroy()
        {
            _fallSequence.Kill();
            _obiEmitter.OnEmitParticle -= OnEmitParticle;
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
            _obiEmitter.OnEmitParticle += OnEmitParticle;
            if(!_obiEmitter.enabled)
                StartCoroutine(LiquidRoutine());
        }

        private void OnEmitParticle(ObiEmitter emitter, int particleIndex)
        {
            _particleCounter++;
        }

        private IEnumerator LiquidRoutine()
        {
            _obiEmitter.enabled = true;
            yield return new WaitForSeconds(2.5f);
            _obiEmitter.OnEmitParticle -= OnEmitParticle;
            _obiEmitter.enabled = false;
        }
    }
}


