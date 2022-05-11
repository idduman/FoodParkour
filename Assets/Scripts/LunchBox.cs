using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dixy.LunchBoxRun
{
    public class LunchBox : MonoBehaviour
    {
        public static event Action ObstacleHit;
        
        private List<SolidFood> _foods = new List<SolidFood>();
        private List<Plate> _plates = new List<Plate>();

        private const float _angleVariaton = 10f;
        private const float _throwSpeed = 4f;
        private const float _foodAlignDuration = 0.25f;
        private const float _jumpAmount = 0.7f;

        public void Awake()
        {
            _foods = GetComponentsInChildren<SolidFood>().ToList();
            _plates = GetComponentsInChildren<Plate>().ToList();
            
            _foods.ForEach(f => f.gameObject.SetActive(false));
        }

        public void OnEnable()
        {
            Plate.FoodHit += OnFoodHit;
        }

        public void OnDisable()
        {
            Plate.FoodHit -= OnFoodHit;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                other.transform.SendMessageUpwards("OpenGate");
                OnObstacleHit();
            }
        }

        private void OnFoodHit(Plate plate, SolidFood food)
        {
            food.IsStatic = true;
            
            var foodsMatched = _foods.Where(x => x.Type == food.Type).ToList();
            if (foodsMatched.Count > 0)
            {
                var foodInside = foodsMatched.FirstOrDefault(x => !x.Placed);
                if (!foodInside)
                    return;
                
                foodInside.IsStatic = true;
                foodInside.Placed = true;
                food.transform.parent = foodInside.transform.parent;
                food.transform.DOLocalRotateQuaternion(foodInside.transform.localRotation, _foodAlignDuration);
                food.transform.DOLocalMove(foodInside.transform.localPosition, _foodAlignDuration)
                    .OnComplete(() =>
                    {
                        foodInside.gameObject.SetActive(true);
                        Destroy(food.gameObject);
                    });
            }
            else
            {
                RemoveFoodFromPlate(plate);
                Destroy(food.gameObject);
            }
        }

        private void OnObstacleHit()
        {
            ObstacleHit?.Invoke();
            _plates.ForEach(RemoveFoodFromPlate);
        }
        

        private void RemoveFoodFromPlate(Plate plate)
        {
            if (_foods.Count == 0)
                return;
            
            var foodToRemove = _foods.LastOrDefault(x => x && x.Placed && (x.transform.parent == plate.transform));
            if (foodToRemove == null)
                return;

            foodToRemove.Throw(GenerateRandomVelocity(transform.position, plate.transform.position));
        }

        private Vector3 GenerateRandomVelocity(Vector3 from, Vector3 to)
        {
            var vel = (to - from);
            vel.y = _jumpAmount;
            vel = vel.normalized * _throwSpeed;
            vel = Quaternion.AngleAxis(Random.Range(-_angleVariaton / 2f, _angleVariaton / 2f), Vector3.up) * vel;

            return vel;
        }
        
        
    }
}