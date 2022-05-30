using System;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class BugFood : SolidFood
    {
        public static event Action<BugFood> BugHit;
        protected override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);
            if (other.gameObject.CompareTag("LunchBox") || 
                other.gameObject.CompareTag("SolidFood") && 
                other.gameObject.TryGetComponent<SolidFood>(out var food) &&
                food.Placed &&
                !(food is BugFood))
            {
                Placed = true;
                IsStatic = true;
                var rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                BugHit?.Invoke(this);
            }
            else if(!IsStatic)
            {
                Destroy(gameObject);
            }
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BugFood>(out var bug))
            {
                Destroy(gameObject);
            }
        }*/
    }
}
