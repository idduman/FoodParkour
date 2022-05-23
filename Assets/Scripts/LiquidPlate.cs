using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dixy.LunchBoxRun
{
    public class LiquidPlate : Plate
    {
        public static event Action Filling;
        public event Action FillComplete;

        public LiquidType LiquidType;

        private Vector3 _initialScale;

        public bool Filled
        {
            get;
            private set;
        }

        public float FillAmount;

        [FormerlySerializedAs("_soupTransform")] [SerializeField] private Transform _liquidTransform;

        private void Start()
        {
            _initialScale = _liquidTransform.localScale;
            _liquidTransform.localScale = new Vector3(_initialScale.x, 0f, _initialScale.z);
            _liquidTransform.gameObject.SetActive(false);
            FillAmount = 0f;
        }

        public void FillLiquid()
        {
            if (Filled)
                return;
            
            FillAmount = 1f;
            Filling?.Invoke();
            _liquidTransform.gameObject.SetActive(true);
            _liquidTransform.DOScaleY(_initialScale.y, 0.5f)
                .OnComplete(() => FillComplete?.Invoke());
        }
    }
}

