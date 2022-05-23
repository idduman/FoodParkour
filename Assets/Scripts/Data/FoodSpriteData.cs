using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Dixy.LunchBoxRun
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public class FoodSpriteData : ScriptableObject
    {
        [Serializable]
        public struct FoodSprite
        {
            public FoodType FoodType;
            public Sprite Sprite;
        }
        [Serializable]
        public struct LiquidSprite
        {
            public LiquidType LiquidType;
            public Sprite Sprite;
        }
        
        private const string TypeName = nameof(FoodSpriteData);
        private const string MenuName = "Data/" + TypeName;

        public List<FoodSprite> _solidFoodSprites;
        public List<LiquidSprite> _liquidFoodSprites;

        public Sprite GetSolidFoodSprite(FoodType foodType)
        {
            return _solidFoodSprites.FirstOrDefault(x => x.FoodType == foodType).Sprite;
        }

        public Sprite GetLiquidFoodSprite(LiquidType liquidType)
        {
            return _liquidFoodSprites.FirstOrDefault(x => x.LiquidType == liquidType).Sprite;
        }
    }
}

