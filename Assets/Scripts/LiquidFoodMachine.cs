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
        [SerializeField] private LiquidType _liquidType;
        [SerializeField] private Transform _nozzleTransform;

        private bool _started;
        private bool _flowing;

        //private Painter _painter;
        private LayerMask _plateMask;
        private LiquidPlate liquidPlate;
        
        private void Awake()
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
            _plateMask = LayerMask.GetMask("Plate");
            var liquid = Instantiate(GameManager.Instance.FoodPrefabData.GetLiquidStream(_liquidType), _nozzleTransform);
            liquid.transform.localPosition = Vector3.zero;
            
            foreach (var s in _sprites)
            {
                s.sprite = GameManager.Instance.FoodSpriteData.GetLiquidFoodSprite(_liquidType);
            }
        }

        private void OnEnable()
        {
            GameManager.LevelLoaded += OnLevelLoaded;
        }

        private void OnDisable()
        {
            GameManager.LevelLoaded -= OnLevelLoaded;
            if(liquidPlate)
                liquidPlate.FillComplete -= OnFillComplete;
        }
    
        private void OnLevelLoaded()
        {
            _started = true;
            _flowing = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Plate") 
                && other.TryGetComponent<LiquidPlate>(out liquidPlate)
                && liquidPlate.LiquidType == _liquidType)
            {
                liquidPlate.FillComplete += OnFillComplete;
                liquidPlate.FillLiquid();
            }
        }

        private void OnFillComplete()
        {
            _flowing = false;
        }

        protected override void Update()
        {
            base.Update();
            _nozzleTransform.gameObject.SetActive(_started && _flowing && Active);

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
