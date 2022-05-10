using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HyperCore
{
    public class UIController : SingletonBehaviour<UIController>
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private RectTransform _successPanel;
        [SerializeField] private RectTransform _failPanel;
        [SerializeField] private RectTransform _tutorialPanel;

        public void ActivateEndgamePanel(bool success)
        {
            if(success)
                _successPanel.gameObject.SetActive(true);
            else
                _failPanel.gameObject.SetActive(true);
        }

        public void LoadButton()
        {
            _successPanel.gameObject.SetActive(false);
            _failPanel.gameObject.SetActive(false);
            
            GameManager.Instance.LoadLevel();
        }

        public void ToggleTutorialPanel(bool active)
        {
            _tutorialPanel.gameObject.SetActive(active);
        }

        public void SetLevelText(int levelIndex)
        {
            _levelText.text = $"Level {levelIndex+1}";
        }
    }
}

