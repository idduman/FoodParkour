using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dixy.FoodParkour
{
    [Serializable]
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public class GameConfig : ScriptableObject
    {
        private const string TypeName = nameof(GameConfig);
        private const string MenuName = "Data/" + TypeName;
        
        public float ConveyorMoveSpeed;
        public float BarrierToggleDuration;
        public float FoodFlySpeed;
        public Vector3 FoodFlyVector;
        public float TrainMoveDuration;
        public float SoupFallDuration;
        public float SoupTurnDuration;
    }

}
