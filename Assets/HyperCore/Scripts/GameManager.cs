using System;
using System.Collections;
using System.Collections.Generic;
using Dixy.LunchBoxRun;
using UnityEngine;

namespace HyperCore
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public static event Action LevelLoaded;
        public FoodPrefabData FoodPrefabData;
        public FoodSpriteData FoodSpriteData;
        
        [SerializeField] private List<LevelBehaviour> _levels = new List<LevelBehaviour>();

        public int CurrentLevel;

        private LevelBehaviour _level;

        private void Start()
        {
            if (_levels.Count < 1)
            {
                Debug.LogError("No levels found");
                return;
            }
            CurrentLevel = PlayerPrefs.GetInt("PlayerLevel", 0);
            LoadLevel();
        }

        public void LoadLevel()
        {
            if(_level)
                DestroyImmediate(_level.gameObject);

            var index = CurrentLevel % _levels.Count;
            
            _level = Instantiate(_levels[index]);
            UIController.Instance.SetLevelText(index);
            if(index == 0)
                UIController.Instance.ToggleTutorialPanel(true);
            
            LevelLoaded?.Invoke();
        }

        public void FinishGame(bool success)
        {
            if (success)
                CurrentLevel++;
            
            PlayerPrefs.SetInt("PlayerLevel", CurrentLevel);
            
            UIController.Instance.ActivateEndgamePanel(success);
        }
    }
}

