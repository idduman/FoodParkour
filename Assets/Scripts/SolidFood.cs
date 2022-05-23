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

        private void OnCollisionEnter(Collision other)
        {
            if (IsStatic)
                return;

            if (!other.collider.CompareTag("LunchBox"))
            {
                Destroy(gameObject);
            }
        }

        public void Throw(Vector3 velocity)
        {
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(velocity, ForceMode.VelocityChange);
            rb.AddTorque(velocity*10f, ForceMode.VelocityChange);
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
        ColaGlass,
    }
}

