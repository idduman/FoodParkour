using System;
using System.Collections;
using System.Collections.Generic;
using HyperCore;
using HyperCore.Runner;
using UnityEngine;
using UnityEngine.Rendering;

namespace Dixy.LunchBoxRun
{
    public class LiquidFoodMachine : FoodMachine
    {
        [SerializeField] private GameObject _soupStream;

        private bool _started;
        private bool _flowing;

        //private Painter _painter;
        private LayerMask _plateMask;
        private SoupPlate _soupPlate;

        private void Start()
        {
            //_painter = GetComponentInChildren<Painter>();
            _plateMask = LayerMask.GetMask("Plate");
        }

        private void OnEnable()
        {
            GameManager.LevelLoaded += OnLevelLoaded;
        }

        private void OnDisable()
        {
            GameManager.LevelLoaded -= OnLevelLoaded;
            if(_soupPlate)
                _soupPlate.FillComplete -= OnFillComplete;
        }
    
        private void OnLevelLoaded()
        {
            _started = true;
            _flowing = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Plate") && other.TryGetComponent<SoupPlate>(out _soupPlate))
            {
                _soupPlate.FillComplete += OnFillComplete;
                _soupPlate.FillSoup();
            }
        }

        private void OnFillComplete()
        {
            _flowing = false;
        }

        protected override void Update()
        {
            base.Update();
            _soupStream.gameObject.SetActive(_started && _flowing && Active);

            /*if (!_flowing)
                return;

            Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out var hit, 5f, _plateMask) && hit.collider.TryGetComponent<SoupPlate>(out var soupPlate))
            {
                soupPlate.FillSoup();
                _flowing = false;
            }*/
        }
    }
}
