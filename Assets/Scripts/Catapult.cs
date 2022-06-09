using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCore;
using UnityEditor;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class Catapult : MonoBehaviour
    {
        [SerializeField] private Transform _armature;
        [SerializeField] private Food _cupCake;
        private int _machineLayer;

        private Tweener _catapultTween;

        private bool _triggered;
        private bool _launched;

        private void Start()
        {
            _machineLayer = LayerMask.NameToLayer("Machine");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered && other.gameObject.layer == _machineLayer)
            {
                _triggered = true;
                _catapultTween = _armature.DOLocalRotate(new Vector3(-90f, 0f, 0f), GameManager.Instance.Config.CatapultLaunchDuration)
                    .SetEase(Ease.OutElastic)
                    .OnUpdate(() =>
                    {
                        if (!_launched && _catapultTween.ElapsedPercentage() > 0.015f)
                        {
                            _launched = true;
                            _cupCake.Throw(GameManager.Instance.Config.CatapultThrowSpeed * GameManager.Instance.Config.CatapultThrowVector.normalized);
                        }
                    });
            }
        }
    }
}

