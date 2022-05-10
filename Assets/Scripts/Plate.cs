using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class Plate : MonoBehaviour
    {
        private List<SolidFood> _foods = new List<SolidFood>();

        public void Start()
        {
            _foods = GetComponentsInChildren<SolidFood>().ToList();
            _foods.ForEach(f => f.gameObject.SetActive(false));
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SolidFood") && 
                other.TryGetComponent<SolidFood>(out var food) &&
                !food.IsStatic)
            {
                AddFood(food);
            }
        }

        public void AddFood(SolidFood food)
        {
            var foodInside = _foods.FirstOrDefault(x => !x.Placed && (x.Type == food.Type));
            if (!foodInside)
            {
                var firstEmpty = _foods.FirstOrDefault(x => !x.Placed);
                if (!firstEmpty)
                    return;

                food.IsStatic = true;
                food.GetComponent<Rigidbody>().isKinematic = true;
                food.transform.parent = transform;
                food.transform.position = firstEmpty.transform.position;
                firstEmpty.Placed = true;
                return;
            }
            
            foodInside.gameObject.SetActive(true);
            foodInside.Placed = true;
            Destroy(food.gameObject);
        }
    }
}