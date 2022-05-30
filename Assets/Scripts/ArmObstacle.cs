using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class ArmObstacle : MonoBehaviour
    {
        [SerializeField] private Transform _elbowTransform;
        [SerializeField] private float _rotateDuration;
        [SerializeField] private float _waitDuration;

        private Sequence _rotateSequence;

        private void Start()
        {
            _rotateSequence = DOTween.Sequence();
            _rotateSequence.Append(_elbowTransform.DOLocalRotate(new Vector3(0f, 0f, -80f), _rotateDuration));
            _rotateSequence.AppendInterval(_waitDuration);
            _rotateSequence.Append(_elbowTransform.DOLocalRotate(Vector3.zero, _rotateDuration));
            _rotateSequence.AppendInterval(_waitDuration);
            _rotateSequence.SetLoops(-1);
            _rotateSequence.Play();
        }

        private void OnDestroy()
        {
            _rotateSequence.Kill();
        }
    }

}
