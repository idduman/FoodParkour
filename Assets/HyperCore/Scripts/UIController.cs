using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCore
{
    public class UIController : SingletonBehaviour<UIController>
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _levelBar;
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

        public void SetLevelPercentage(float percentage, bool instant = false)
        {
            if(instant)
            {
                _levelBar.value = percentage;
                return;
            }
            
            DOTween.To(() => _levelBar.value, x => _levelBar.value = x, percentage, 0.2f)
                .SetEase(Ease.OutQuad);
        }
    }
}

