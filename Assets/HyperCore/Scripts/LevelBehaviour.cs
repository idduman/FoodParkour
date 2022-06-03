using System;
using Dixy.FoodParkour;
using UnityEngine;

namespace HyperCore
{
    public class LevelBehaviour : MonoBehaviour
    {
        [SerializeField] private ConveyorBelt _conveyorBelt;
        public ConveyorBelt ConveyorBelt => _conveyorBelt;
        public Material Skybox;

        public void Awake()
        {
            _conveyorBelt = GetComponentInChildren<ConveyorBelt>();
        }
    }
}