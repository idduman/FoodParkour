using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        [SerializeField] private RectTransform _endgamePanel;
        [SerializeField] private RectTransform _successPanel;
        [SerializeField] private RectTransform _failPanel;
        [SerializeField] private RectTransform _boxPanel;
        [SerializeField] private RectTransform _tutorialPanel;
        [SerializeField] private Image _lunchboxSprite;
        [SerializeField] private TextMeshProUGUI _lunchboxText;
        [SerializeField] private Image _endgameFill;
        [SerializeField] private TextMeshProUGUI _endgameText;

        private static readonly float _endgmaeFillDuration = 1.25f;

        private void Awake()
        {
            _levelBar.gameObject.SetActive(true);
            _endgamePanel.gameObject.SetActive(false);
            _successPanel.gameObject.SetActive(false);
            _failPanel.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_endgameText.gameObject.activeInHierarchy)
            {
                var amount = Mathf.Ceil(_endgameFill.fillAmount * 100f);
                _endgameText.text = $"{amount}%";
            }
        }

        public void ActivateEndgamePanel(bool success)
        {
            StartCoroutine(EndGameRoutine(success));
        }

        public void LoadButton()
        {
            _levelBar.gameObject.SetActive(true);
            _endgamePanel.gameObject.SetActive(false);
            _successPanel.gameObject.SetActive(false);
            _failPanel.gameObject.SetActive(false);
            
            GameManager.Instance.LoadLevel();
        }

        public void ToggleTutorialPanel(bool active)
        {
            _tutorialPanel.gameObject.SetActive(active);
        }

        public void ToggleBoxPanel(bool active)
        {
            _boxPanel.gameObject.SetActive(active);
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

        public void SetLunchboxPanel(Sprite sprite, String name)
        {
            _lunchboxSprite.sprite = sprite;
            _lunchboxText.text = name;
        }

        private void EndgameFill()
        {
            _endgameFill.fillAmount = 0f;
            _endgameText.text = "0%";
            _endgameFill.gameObject.SetActive(true);
            _endgameText.gameObject.SetActive(true);
            DOTween.To(() => _endgameFill.fillAmount, x => _endgameFill.fillAmount = x,
                    _levelBar.value, _endgmaeFillDuration)
                .SetEase(Ease.InOutQuad);
        }

        private IEnumerator EndGameRoutine(bool success)
        {
            _levelBar.gameObject.SetActive(false);
            _endgamePanel.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(0.1f);
            
            EndgameFill();

            yield return new WaitForSeconds(_endgmaeFillDuration);
            
            //Confetti

            yield return new WaitForSeconds(0.5f);
            
            _successPanel.gameObject.SetActive(success);
            _failPanel.gameObject.SetActive(!success);
        }
    }
}

