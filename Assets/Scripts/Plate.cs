using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class Plate : MonoBehaviour
    {
        public static event Action<Plate, SolidFood> FoodHit;
        
        public List<SolidFood> Foods = new List<SolidFood>();

        public bool IsEmpty => Foods.Count == 0;
        public float FillPercentage => IsEmpty ? 0f : Foods.Count(x => x.Placed) / (float)Foods.Count;

        private void Awake()
        {
            Foods = GetComponentsInChildren<SolidFood>().ToList();
        }
        
        public void OnTriggerEnter(Collider other)
        {
            var rb = other.attachedRigidbody;
            if (!rb)
                return;
            
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

