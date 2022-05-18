using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class SoupPlate : Plate
    {
        public static event Action Filling;
        public event Action FillComplete; 

        public bool Filled
        {
            get;
            private set;
        }

        public float FillAmount => Filled ? 1f : 0f;

        [SerializeField] private Transform _soupTransform;

        private void Start()
        {
            _soupTransform.localPosition = new Vector3(_soupTransform.localPosition.x,
                _soupTransform.localPosition.y, _soupTransform.localPosition.z - 0.1f);
            Filled = false;
        }

        public void FillSoup()
        {
            if (Filled)
                return;

            Filling?.Invoke();
            Filled = true;
            _soupTransform.DOLocalMoveZ(_soupTransform.localPosition.z + 0.1f, 0.5f)
                .OnComplete(() => FillComplete?.Invoke());
        }
    }
}

