using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

namespace HyperCore
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public static event Action LevelLoaded;
        public static event Action Finished;
        
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
            
            StartCoroutine(LoadRoutine());
        }

        public void LoadLevel()
        {
            if(_level)
                DestroyImmediate(_level.gameObject);

            var index = CurrentLevel % _levels.Count;
            
            _level = Instantiate(_levels[index]);
            if (_level.Skybox)
            {
                RenderSettings.skybox = _level.Skybox;
            }
            
            UIController.Instance.SetLevelText(index);
            if(index == 0)
                UIController.Instance.ToggleTutorialPanel(true);
            
            LevelLoaded?.Invoke();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void FinishGame(bool success)
        {
            GameAnalytics.NewProgressionEvent(success ? GAProgressionStatus.Complete : GAProgressionStatus.Fail, CurrentLevel.ToString());
            
            Finished?.Invoke();
            
            if (success)
                CurrentLevel++;
            
            PlayerPrefs.SetInt("PlayerLevel", CurrentLevel);
            
            StartCoroutine(FinishRoutine(success));
        }

        private void EndGame(bool success)
        {
            UIController.Instance.ActivateEndgamePanel(success);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator FinishRoutine(bool success)
        {
            CameraController.Instance.ActivateEndgameCamera();
            yield return new WaitForSeconds(1.5f);
            EndGame(success);
            yield return null;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadRoutine()
        {
            yield return new WaitForSeconds(1);
            
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, CurrentLevel.ToString());
            
            LoadLevel();
        }
    }
}

