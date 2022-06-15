using System;
using DG.Tweening;
using HyperCore;
using Obi;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Renderer _faceRenderer;
        [SerializeField] private Transform _mouthTrigger;
        [SerializeField] private bool _keepMouthOpen;
        [SerializeField] private ParticleSystem _happyParticles;
        [SerializeField] private ParticleSystem _sadParticles;

        private Animator _anim;
        private Transform _finish;
        private bool _finished;
        private Tweener _mouthTween;
        private Tweener _faceTween;
        private Vector3 _mouthScale;
        private static readonly int MouthOpen = Animator.StringToHash("MouthOpen");

        private float _score;
        private float _maxScore;
        private float _nausea;
        private float _maxNausea;

        private Vector3 _faceHSV;
        private static readonly int Happy = Animator.StringToHash("Happy");
        private static readonly int Sad = Animator.StringToHash("Sad");

        void Start()
        {
            _anim = GetComponent<Animator>();
            _finish = GameObject.FindGameObjectWithTag("Finish").transform;
            if(!_finish)
                Debug.LogError("No finish line found in level!");

            _mouthScale = _mouthTrigger.localScale;
            _mouthTrigger.localScale = new Vector3(_mouthScale.x, 0f, _mouthScale.z);
            _mouthTrigger.gameObject.SetActive(false);
            
            Color.RGBToHSV(_faceRenderer.material.color, out _faceHSV.x,out _faceHSV.y, out _faceHSV.z);
            _score = 0f;
            _score = _maxScore = GameManager.Instance.Config.MaxScore;
            _nausea = 0f;
            _maxNausea = GameManager.Instance.Config.MaxNausea;

            Subscribe();
            
        }
        
        private void OnDestroy()
        {
            Unsubscribe();
            StopAllCoroutines();
        }

        void Update()
        {
            if (_finished)
                return;
            
            if(_keepMouthOpen)
                ToggleMouth(true);

            if (transform.position.z > _finish.position.z)
            {
                Finish(true);
                return;
            }
            
        }
        
        private void Subscribe()
        {
            InputController.Instance.Pressed += OnPressed;
            InputController.Instance.Released += OnRelease;
            Food.FoodEaten += OnFoodEaten;
            Barrier.GateOpen += Gesture;
        }

        private void Unsubscribe()
        {
            if (!InputController.Instance)
                return;
            
            InputController.Instance.Pressed -= OnPressed;
            InputController.Instance.Released -= OnRelease;
            Food.FoodEaten -= OnFoodEaten;
            Barrier.GateOpen -= Gesture;
        }

        private void OnRelease(Vector3 obj)
        {
            ToggleMouth(false);
        }

        private void OnPressed(Vector3 obj)
        {
            UIController.Instance.ToggleTutorialPanel(false);
            ToggleMouth(true);
        }
        
        private void OnFoodEaten(float points)
        {
            var pointsNegative = Mathf.Min(points, 0f);
            var pointsPositive = Mathf.Max(points, 0f);

            _nausea = Mathf.Clamp(_nausea - pointsNegative, 0f, _maxNausea);
            _score = Mathf.Clamp(_score + pointsPositive, 0f, _maxScore);
            
            SetFaceColor();
            UIController.Instance.SetLevelPercentage(_score / _maxScore);
        }

        private void Gesture()
        {
            if (_nausea > _maxNausea * 0.95f)
            {
                Finish(false);
            }
            else if (_nausea > _maxNausea * 0.3f)
            {
                _anim.SetTrigger(Sad);
                _sadParticles.Play();
            }
            else
            {
                _anim.SetTrigger(Happy);
                _happyParticles.Play();
            }
        }

        private void SetFaceColor()
        {
            var perc = _nausea / _maxNausea;
            
            _faceHSV.y = Mathf.Lerp(0f, 0.5f, perc);
            var albedo = Color.HSVToRGB(_faceHSV.x, _faceHSV.y, _faceHSV.z);

            _faceTween.Kill();
            _faceTween = _faceRenderer.material.DOColor(albedo, 0.2f)
                .SetEase(Ease.Linear);
        }

        private void Finish(bool success)
        {
            if (_finished)
                return;

            _finished = true;
            StopAllCoroutines();
            Unsubscribe();
            GameManager.Instance.FinishGame(success);
        }

        private void ToggleMouth(bool open)
        {
            var scale = _mouthScale;
            scale.y = open ? _mouthScale.y : 0f;
            _mouthTween.Kill();
            _anim.SetBool(MouthOpen, open);
            _mouthTween = _mouthTrigger.DOScale(scale, 0.15f)
                .OnUpdate(() =>
                {
                    _mouthTrigger.gameObject.SetActive(_mouthTrigger.localScale.y > 0.4f);
                });
        }
    }
}