using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class Mouth : MonoBehaviour
    {
        [SerializeField] private Transform _throat;
        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody.TryGetComponent<Food>(out var food) && !food.Eaten)
            {
                food.OnEaten();
                food.transform.DOMove(_throat.position, 0.5f)
                    .OnComplete(() => Destroy(food.gameObject, 0.05f));
            }
        }
    }

}
