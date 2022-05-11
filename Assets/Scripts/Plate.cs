using System;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class Plate : MonoBehaviour
    {
        public static event Action<Plate, SolidFood> FoodHit;
        public void OnTriggerEnter(Collider other)
        {
            var rb = other.attachedRigidbody;
            if (rb.CompareTag("SolidFood") && 
                rb.TryGetComponent<SolidFood>(out var food) &&
                !food.IsStatic &&
                !food.Placed)
            {
                FoodHit?.Invoke(this, food);
            }
        }
    }
}

