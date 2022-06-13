using DG.Tweening;
using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _mouthTrigger;
        [SerializeField] private bool _keepMouthOpen;

        private Animator _anim;
        private Transform _finish;
        private bool _finished;
        private Tweener _mouthTween;
        private Vector3 _mouthScale;
        private static readonly int MouthOpen = Animator.StringToHash("MouthOpen");

        void Start()
        {
            _anim = GetComponent<Animator>();
            _finish = GameObject.FindGameObjectWithTag("Finish").transform;
            if(!_finish)
                Debug.LogError("No finish line found in level!");

            _mouthScale = _mouthTrigger.localScale;
            _mouthTrigger.localScale = new Vector3(_mouthScale.x, 0f, _mouthScale.z);
            _mouthTrigger.gameObject.SetActive(false);
            
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
                Finish(true);
        }
        
        private void Subscribe()
        {
            InputController.Instance.Pressed += OnPressed;
            InputController.Instance.Released += OnRelease;
            InputController.Instance.Moved += OnMoved;
        }

        private void Unsubscribe()
        {
            if (!InputController.Instance)
                return;
            
            InputController.Instance.Pressed -= OnPressed;
            InputController.Instance.Released -= OnRelease;
            InputController.Instance.Moved -= OnMoved;
        }

        private void OnMoved(Vector3 obj)
        {
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