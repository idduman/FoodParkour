using System.Collections;
using System.Collections.Generic;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class DeactivateOnFinish : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.Finished += OnFinish;
        }

        private void OnDisable()
        {
            GameManager.Finished -= OnFinish;
        }
        
        private void OnFinish()
        {
            GameManager.Finished -= OnFinish;
            gameObject.SetActive(false);
        }
    }
}

