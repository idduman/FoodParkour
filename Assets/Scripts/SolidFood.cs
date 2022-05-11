using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    [RequireComponent(typeof(Rigidbody))]
    public class SolidFood : MonoBehaviour
    {
        public FoodType Type;
        public bool IsStatic = false;
        public bool Placed = false;

        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsStatic)
                return;

            if (other.collider.CompareTag("Machine") || other.collider.CompareTag("Platform"))
            {
                Destroy(gameObject);
            }
        }

        public void Throw(Vector3 velocity)
        {
            _rb.isKinematic = false;
            _rb.AddForce(velocity, ForceMode.VelocityChange);
            _rb.AddTorque(velocity*10f, ForceMode.VelocityChange);
            IsStatic = false;
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
        Bread,
        Cake,
        Cheese,
        Cookie,
        Fries,
        HotDog,
        Sausage,
        Bug,
    }
}

