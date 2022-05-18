using System;
using DG.Tweening;
using HyperCore.Runner;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class FoodMachine : MonoBehaviour
    {
        public bool Active;

        protected virtual void Update()
        {
            var dist =  transform.position.z - RunnerPlayerBehaviour.ZCoordinate;
        
            Active = (dist < 10f && dist > -1.5f);
        }
    }

}
