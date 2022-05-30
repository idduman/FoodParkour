using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using HyperCore;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dixy.LunchBoxRun
{
    public class LunchBox : MonoBehaviour
    {
        [SerializeField] private String _name;
        [SerializeField] private Sprite _sprite;
        public static event Action ObstacleHit;

        private List<SolidFood> _solidFoods = new List<SolidFood>();
        private List<Plate> _solidPlates = new List<Plate>();
        private List<LiquidPlate> _liquidPlates = new List<LiquidPlate>();
        
        private Tweener _shakeTween;
        private int _foodCount;
        private int _bugCount;

        public float FoodPercentage =>
            Mathf.Clamp(
                (_solidPlates.Sum(x => x.FillPercentage) 
                 + _liquidPlates.Sum(x => x.FillAmount))
                / (_solidPlates.Count(x => !x.IsEmpty) + _liquidPlates.Count + _bugCount/5f),
                0f, 1f);

        private const float _angleVariaton = 15f;
        private const float _throwSpeed = 7f;
        private const float _foodAlignDuration = 0.25f;
        private const float _jumpAmount = 0.8f;
        public void Awake()
        {
            _solidPlates = GetComponentsInChildren<Plate>().ToList();
            _liquidPlates = GetComponentsInChildren<LiquidPlate>().ToList();

            UIController.Instance.SetLevelPercentage(0f, true);
            _foodCount = 0;
            _bugCount = 0;

            UIController.Instance.SetLunchboxPanel(_sprite, _name);
        }

        public void Start()
        {
            foreach (var plate in _solidPlates)
            {
                _solidFoods.AddRange(plate.Foods);
            }
            _solidFoods.ForEach(f => f.gameObject.SetActive(false));
        }

        public void OnEnable()
        {
            Plate.FoodHit += OnFoodHit;
            Plate.ArmHit += OnArmHit;
            BugFood.BugHit += OnBugHit;
            LiquidPlate.Filling += OnFill;
        }

        public void OnDisable()
        {
            Plate.FoodHit -= OnFoodHit;
            Plate.ArmHit -= OnArmHit;
            BugFood.BugHit -= OnBugHit;
            LiquidPlate.Filling -= OnFill;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                other.transform.SendMessageUpwards("OpenGate", SendMessageOptions.DontRequireReceiver);
                OnObstacleHit();
            }
        }

        private void OnFoodHit(Plate plate, SolidFood food)
        {
            if (food is BugFood)
                return;

            var foodsMatched = _solidFoods.Where(x => x.Type == food.Type).ToList();
            if (foodsMatched.Count > 0)
            {
                var foodInside = foodsMatched.FirstOrDefault(x => !x.Placed);
                if (!foodInside)
                {
                    Destroy(food.gameObject);
                    return;
                }

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
                Destroy(food.gameObject);
                if (_shakeTween is {active: true})
                    return;
                Shake(0.1f);
                RemoveFoodFromPlate(plate);
            }
            UIController.Instance.SetLevelPercentage(FoodPercentage);
        }
        
        private void OnBugHit(BugFood bug)
        {
            bug.transform.SetParent(transform);
            _bugCount++;
            UIController.Instance.SetLevelPercentage(FoodPercentage);
        }

        private void OnArmHit(Plate plate, Collider other)
        {
            if (_shakeTween is {active: true})
                return;

            if (!RemoveFoodFromPlate(plate, other))
                return;
            
            Shake(0.1f);
            UIController.Instance.SetLevelPercentage(FoodPercentage);
        }

        private void OnObstacleHit()
        {
            if (_shakeTween is {active: true})
                return;
            
            Shake(0.2f);
            ObstacleHit?.Invoke();
            _solidPlates.ForEach(x => RemoveFoodFromPlate(x));
            UIController.Instance.SetLevelPercentage(FoodPercentage);
        }
        
        private void OnFill()
        {
            UIController.Instance.SetLevelPercentage(FoodPercentage);
        }

        private void Shake(float strength)
        {
            _shakeTween.Kill();
            _shakeTween = transform.DOShakePosition(0.2f, strength, 40, 15);
        }
        

        private SolidFood RemoveFoodFromPlate(Plate plate, Collider other = null)
        {
            if (_solidFoods.Count == 0 || _foodCount == 0)
                return null;
            
            var foodToRemove = _solidFoods.LastOrDefault(x => x && x.Placed && (x.transform.parent == plate.transform));
            if (foodToRemove == null)
                return null;
            
            var foodToThrow = Instantiate(foodToRemove, foodToRemove.transform.position, foodToRemove.transform.rotation);
            foodToThrow.transform.parent = transform.parent.parent.parent;
            foodToThrow.Throw(GenerateRandomVelocity(other ? other.transform.position : transform.position,
                foodToThrow.transform.position));
            
            foodToRemove.Placed = false;
            foodToRemove.gameObject.SetActive(false);

            var liquid = foodToRemove.GetComponentInChildren<LiquidPlate>();
            if(liquid)
                liquid.ResetLiquid();

            _foodCount = Mathf.Max(_foodCount - 1, 0);
            
            return foodToRemove;
        }

        private Vector3 GenerateRandomVelocity(Vector3 from, Vector3 to)
        {
            var vel = (to - from);
            vel.Normalize();
            vel.y = _jumpAmount;
            vel = vel.normalized * _throwSpeed;
            vel = Quaternion.AngleAxis(Random.Range(-_angleVariaton / 2f, _angleVariaton / 2f), Vector3.up) * vel;

            return vel;
        }
        
    }
}