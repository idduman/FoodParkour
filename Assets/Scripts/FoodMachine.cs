using System;
using DG.Tweening;
using HyperCore.Runner;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class FoodMachine : MonoBehaviour
    {
        public bool Active;
        protected SpriteRenderer[] _sprites;

        private static readonly float ClipDist = -1.25f;

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            /*var cameraDist = Vector3.Distance(transform.position , _cameraTransform.position);
            if (cameraDist < _clipDistance)
            {
                Active = false;
                gameObject.SetActive(false);
                return;
            }*/

            var dist =  transform.position.z - RunnerPlayerBehaviour.ZCoordinate;

            Active = (dist < 10f && dist > ClipDist);
            gameObject.SetActive(dist > ClipDist);
        }
    }

}
