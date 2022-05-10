using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class LunchBox : MonoBehaviour
    {
        private List<SolidFood> _foods = new List<SolidFood>();

        public void Start()
        {
            _foods = GetComponentsInChildren<SolidFood>().ToList();
            _foods.ForEach(f => f.gameObject.SetActive(false));
        }

        public void OnTriggerEnter(Collider other)
        {
            var rb = other.attachedRigidbody;
            if (rb.CompareTag("SolidFood") && 
                rb.TryGetComponent<SolidFood>(out var food) &&
                !food.IsStatic)
            {
                AddFood(food);
            }
        }

        public void AddFood(SolidFood food)
        {
            var foodInside = _foods.FirstOrDefault(x => !x.Placed && (x.Type == food.Type));
            if (foodInside)
            {
                foodInside.gameObject.SetActive(true);
                foodInside.IsStatic = true;
                foodInside.Placed = true;
            }
            
            Destroy(food.gameObject);
        }
    }
}