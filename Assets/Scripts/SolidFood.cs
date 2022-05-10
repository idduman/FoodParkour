using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class SolidFood : MonoBehaviour
    {
        public FoodType Type;
        public bool IsStatic = false;
        public bool Placed = false;
        private void OnCollisionEnter(Collision other)
        {
            if (IsStatic)
                return;

            if (other.collider.CompareTag("Machine") || other.collider.CompareTag("Platform"))
            {
                IsStatic = true;
                Destroy(gameObject, 0.2f);
            }
        }
    }

    public enum FoodType
    {
        None,
        Broccoli,
        Egg,
        Roll,
        Strawberry,
        Tomato,
        Soup,
    }
}

