using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace Dixy.LunchBoxRun
{
    public class GateObstacle : MonoBehaviour
    {
        [SerializeField] private Transform _cubeArmature;
        [SerializeField] private Renderer _cubeRenderer;
        [SerializeField] private Renderer _cylinderRenderer1;
        [SerializeField] private Renderer _cylinderRenderer2;
        
        private GateButton[] _buttons;
        private Material _sharedCubeMaterial;
    
        private const float _gateOpenDuration = 0.5f;
        private void Awake()
        {
            _buttons = GetComponentsInChildren<GateButton>();
            if (!_cubeRenderer || _cubeRenderer.sharedMaterials.Length < 2)
            {
                Debug.LogError("Cube Renderer with 2 materials is missing");
                return;
            }
            _sharedCubeMaterial = _cubeRenderer.sharedMaterials[1];
            
            Subscribe();
        }
        
        private void OnDestroy()
        {
            Unsunbscribe();
        }
    
        private void Subscribe()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Pressed += OnButtonPressed;
            }
        }
    
        private void Unsunbscribe()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Pressed -= OnButtonPressed;
            }
        }
    
        private void OnButtonPressed(Material sharedMaterial)
        {
            Unsunbscribe();
            
            if (_sharedCubeMaterial == sharedMaterial)
            {
                OpenGate();
            }
            /*else
            {
                Material[] materials = new Material[_cubeRenderer.materials.Length];
                materials[0] = _cubeRenderer.materials[0];
                materials[1] = sharedMaterial;
                _cubeRenderer.materials = _cylinderRenderer1.materials = _cylinderRenderer2.materials = materials;
            }*/
        }
    
        public void OpenGate()
        {
            _cubeArmature.DOLocalMoveY(-1f, _gateOpenDuration)
                .SetEase(Ease.Linear);
        }
    }
}
