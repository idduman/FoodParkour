using System;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class Painter : MonoBehaviour
    { 
        [SerializeField] private Color _color;
        public bool Painting = false;

        private P3dHitCache _hitCache = new P3dHitCache();
        
        private LayerMask _foodMask = LayerMask.GetMask("LunchBox", "Food");

        private const bool preview = false;
        private const int priority = Int32.MaxValue;
        private const float pressure = Int32.MaxValue;

        private void Awake()
        {
            
        }
        
        private void Update()
        {
            if (!Painting)
                return;
            Ray ray = new Ray(transform.position, Vector3.down);
            
            if(Physics.Raycast(ray, out var hit, 10f, _foodMask))
            {
                var rotation = Quaternion.LookRotation(hit.normal, Vector3.forward);
                _hitCache.InvokeCoord(hit.collider.gameObject, preview, priority, pressure, new P3dHit(), rotation);
            }
        }
    }
}
