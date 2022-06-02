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
        [SerializeField] private RectTransform _tutorialPanel;
        [SerializeField] private Image _endgameFill;
        [SerializeField] private TextMeshProUGUI _endgameText;

        private static readonly float _endgmaeFillDuration = 1.25f;
        private Vector3 _lunchBoxPos;

        private void Awake()
        {
            ResetUI();
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
            ResetUI();
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

        private void ResetUI()
        {
            _endgamePanel.gameObject.SetActive(false);
            _successPanel.gameObject.SetActive(false);
            _failPanel.gameObject.SetActive(false);
            _levelBar.gameObject.SetActive(true);

            _endgameFill.fillAmount = 0f;
            _endgameText.text = "0%";
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

            yield return new WaitForSeconds(0.5f);
            
            EndgameFill();

            yield return new WaitForSeconds(_endgmaeFillDuration);
            
            //Confetti

            yield return new WaitForSeconds(0.5f);
            
            _successPanel.gameObject.SetActive(success);
            _failPanel.gameObject.SetActive(!success);
        }
    }
}

