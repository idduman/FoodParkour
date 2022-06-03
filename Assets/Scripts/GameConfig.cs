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
    }

}
