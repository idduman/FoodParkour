using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dixy.LunchBoxRun
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public class FoodPrefabData : ScriptableObject
    {
        private const string TypeName = nameof(FoodPrefabData);
        private const string MenuName = "Data/" + TypeName;
        // Start is called before the first frame update

        [FormerlySerializedAs("FoodList")] [SerializeField] private List<SolidFood> SolidFoodList = new List<SolidFood>();
        [SerializeField] private List<LiquidFood> LiquidStreamList = new List<LiquidFood>();

        public SolidFood GetRandomFood(FoodType foodType)
        {
            var filteredList = SolidFoodList.Where(x => x.Type == foodType).ToList();
            return filteredList[Random.Range(0, filteredList.Count-1)];
        }

        public LiquidFood GetLiquidStream(LiquidType liquidType)
        {
            return LiquidStreamList.FirstOrDefault(x => x.FoodType == liquidType);
        }
    }
}

