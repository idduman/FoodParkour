using System.Collections;
using System.Collections.Generic;
using HyperCore;
using HyperCore.Runner;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class LiquidFoodMachine : MonoBehaviour
    {
        private bool _started = false;
        private bool _flowing = false;

        private Painter _painter;

        private void Start()
        {
            _painter = GetComponentInChildren<Painter>();
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
        
        void Update()
        {
            if (!_started)
                return;
            
            var dist =  transform.position.z - RunnerPlayerBehaviour.ZCoordinate;

            _flowing = dist < 10f && dist > -1.5f;

            _painter.Painting = _started && _flowing;
        }
    }
}
