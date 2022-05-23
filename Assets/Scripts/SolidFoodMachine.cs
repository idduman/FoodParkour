using System;
using System.Collections;
using System.Collections.Generic;
using HyperCore;
using HyperCore.Runner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dixy.LunchBoxRun
{
    public class SolidFoodMachine : FoodMachine
    {
        [SerializeField] private FoodType _foodType;
        [SerializeField] private float _dropInterval = 0.5f;
        [SerializeField] private float _dropheight = 1f;

        private float _dropTimer;
        private bool _started = false;
        private void Awake()
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            _dropTimer = _dropInterval + Random.Range(-_dropInterval/2f, _dropInterval/2f);
            foreach (var s in _sprites)
            {
                s.sprite = GameManager.Instance.FoodSpriteData.GetSolidFoodSprite(_foodType);
            }
        }

        private void OnEnable()
        {
            GameManager.LevelLoaded += OnLevelLoaded;
        }

        private void OnDisable()
        {
            GameManager.LevelLoaded -= OnLevelLoaded;
        }
    
        private void OnLevelLoaded()
        {
            _started = true;
        }
        
        protected override void Update()
        {
            base.Update();
            
            if (!_started)
                return;

            if (!Active || _foodType == FoodType.None)
                return;

            if (_dropTimer <= 0f)
            {
                DropItem();
                _dropTimer = _dropInterval;
                return;
            }

            _dropTimer -= Time.deltaTime;
        }

        private void DropItem()
        {
            var foodToDrop = GameManager.Instance.FoodPrefabData.GetRandomFood(_foodType);
            if (!foodToDrop)
            {
                Debug.LogError($"No food of type {_foodType} found in food data");
                return;
            }
            
            Instantiate(foodToDrop, transform.position + _dropheight * Vector3.up, foodToDrop.transform.rotation);
        }
    }
}

