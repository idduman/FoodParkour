using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameAnalyticsSDK.Setup;
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
        [SerializeField] private Transform _foodsParent;

        private Vector3 _armatureRotation;
        private Vector3 _handleRotation;
        private int _machineLayer;
        private bool _triggered;
        private Sequence _fallSequence;
        private List<Transform> _painters = new List<Transform>();
        private List<Food> _foods;
        private Transform _particlePool;
        private float _flySpeed;

        private int _particleCounter;
        
        private void Start()
        {
            _armatureRotation = _armature.localRotation.eulerAngles;
            _machineLayer = LayerMask.NameToLayer("Machine");
            _obiEmitter.enabled = false;
            _flySpeed = GameManager.Instance.Config.FoodFlySpeed;
            
            _particlePool = transform.Find("Pool");
            for (int i = 0; i < GameManager.Instance.Config.SoupPainterParticleAmount; i++)
            {
                _painters.Add(Instantiate(_painterPrefab, _particlePool).transform);
            }

            _foods = _foodsParent.GetComponentsInChildren<Food>().ToList();
            StartCoroutine(FoodDeactivate());
        }
        
        private void Update()
        {
            if (!_obiEmitter.enabled)
                return;

            for (int i = 0; i < _painters.Count; i++)
            {
                var index = (i * 20) % _particleCounter;
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
            if(_particleCounter % 5 == 0)
                ThrowRandomFood();
        }

        private void ThrowRandomFood()
        {
            if (_foods.Count < 1)
                return;

            var index = Random.Range(0, _foods.Count);
            var foodToThrow = _foods[index];
            _foods.RemoveAt(index);
            foodToThrow.gameObject.SetActive(true);
            foodToThrow.Throw(_obiEmitter.transform.forward * _flySpeed, true);
        }

        private IEnumerator LiquidRoutine()
        {
            _obiEmitter.enabled = true;
            yield return new WaitForSeconds(2.5f);
            _obiEmitter.OnEmitParticle -= OnEmitParticle;
            _obiEmitter.enabled = false;
        }

        private IEnumerator FoodDeactivate()
        {
            yield return new WaitForSeconds(0.5f);
            _foods.ForEach(food => food.gameObject.SetActive(false));
        }
    }
}


