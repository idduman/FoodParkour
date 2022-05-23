using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using HyperCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dixy.LunchBoxRun
{
    public class LunchBox : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        public static event Action ObstacleHit;

        private List<SolidFood> _solidFoods = new List<SolidFood>();
        private List<Plate> _solidPlates = new List<Plate>();
        private List<LiquidPlate> _liquidPlates = new List<LiquidPlate>();

        private int _foodCount;
        private float FoodPercentage => (float)_foodCount / _solidFoods.Count;

        private const float _angleVariaton = 20f;
        private const float _throwSpeed = 5f;
        private const float _foodAlignDuration = 0.25f;
        private const float _jumpAmount = 0.7f;
        public void Awake()
        {
            _solidFoods = GetComponentsInChildren<SolidFood>().ToList();
            _solidPlates = GetComponentsInChildren<Plate>().ToList();
            _liquidPlates = GetComponentsInChildren<LiquidPlate>().ToList();

            UIController.Instance.SetLevelPercentage(0f, true);
            _foodCount = 0;
            
            _solidFoods.ForEach(f => f.gameObject.SetActive(false));
            
            UIController.Instance.SetLunchboxSprite(_sprite);
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
            var foodsMatched = _solidFoods.Where(x => x.Type == food.Type).ToList();
            if (foodsMatched.Count > 0)
            {
                var foodInside = foodsMatched.FirstOrDefault(x => !x.Placed);
                if (!foodInside)
                    return;
                
                food.IsStatic = true;
                foodInside.IsStatic = true;
                foodInside.Placed = true;
                _foodCount = Mathf.Min(_foodCount + 1, _solidFoods.Count);
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
            UIController.Instance.SetLevelPercentage(FoodPercentage);
        }

        private void OnObstacleHit()
        {
            ObstacleHit?.Invoke();
            _solidPlates.ForEach(RemoveFoodFromPlate);
            UIController.Instance.SetLevelPercentage(FoodPercentage);
        }
        

        private void RemoveFoodFromPlate(Plate plate)
        {
            if (_solidFoods.Count == 0 || _foodCount == 0)
                return;
            
            var foodToRemove = _solidFoods.LastOrDefault(x => x && x.Placed && (x.transform.parent == plate.transform));
            if (foodToRemove == null)
                return;
            
            var foodToThrow = Instantiate(foodToRemove, foodToRemove.transform.position, foodToRemove.transform.rotation);
            foodToThrow.transform.parent = transform.parent.parent.parent;
            foodToThrow.Throw(GenerateRandomVelocity(transform.position, plate.transform.position));
            
            foodToRemove.Placed = false;
            foodToRemove.gameObject.SetActive(false);

            _foodCount = Mathf.Max(_foodCount - 1, 0);
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