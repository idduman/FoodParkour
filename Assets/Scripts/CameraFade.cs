using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class CameraFade : MonoBehaviour
    {
        [SerializeField] private GameObject[] _clipObjects;
        private Transform _cameraTransform;
        private static readonly int Color1 = Shader.PropertyToID("_Color");
        private static readonly float ClipDistance = 5f;

        private float _clipDistance;
        private bool _clipped = false;

        void Start()
        {
            var camera = Camera.main;
            _cameraTransform = camera.transform;
            _clipDistance = camera.nearClipPlane;
        }
        
        void Update()
        {
            var zDist = Vector3.Distance(transform.position , _cameraTransform.position);
            if (zDist < _clipDistance && !_clipped)
            {
                foreach (var obj in _clipObjects)
                {
                    obj.SetActive(false);
                }
                _clipped = true;
            }
        }
    }
}

