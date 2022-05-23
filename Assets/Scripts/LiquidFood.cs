using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class LiquidFood : MonoBehaviour
    {
        public LiquidType FoodType;
    }
    public enum LiquidType
    {
        None,
        Soup,
        Ketchup,
        Coke,
    }
}

