using System;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class Painter : MonoBehaviour
    { 
        public P3dPaintDecal MyDecal;
        public bool Painting = false;

        private P3dHitCache _hitCache = new P3dHitCache();

        private LayerMask _foodMask;

        private const bool Preview = false;
        private const int Priority = 0;
        private const float Pressure = 1.0f;
        private const int Seed = 0;

        private void Awake()
        {
            _foodMask = LayerMask.GetMask("LunchBox", "Food");
            Painting = false;
        }
        
        private void Update()
        {
            if (!Painting)
                return;
            
            Ray ray = new Ray(transform.position, Vector3.down);
            if(Physics.Raycast(ray, out var hit, 10f, _foodMask))
            {
                var rotation = Quaternion.LookRotation(-hit.normal);
                MyDecal.HandleHitPoint(Preview, Priority, Pressure, Seed, hit.point, rotation);
            }
        }
    }
}
