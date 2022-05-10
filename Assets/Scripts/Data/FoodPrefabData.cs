using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public class FoodPrefabData : ScriptableObject
    {
        private const string TypeName = nameof(FoodPrefabData);
        private const string MenuName = "Data/" + TypeName;
        // Start is called before the first frame update

        [SerializeField] private List<SolidFood> FoodList = new List<SolidFood>();

        public SolidFood GetRandomFood(FoodType foodType)
        {
            var filteredList = FoodList.Where(x => x.Type == foodType).ToList();
            return filteredList[Random.Range(0, filteredList.Count-1)];
        }
    }
}

