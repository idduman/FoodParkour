using DG.Tweening;
using Dixy.LunchBoxRun;
using UnityEngine;

namespace HyperCore.Runner
{
    public class RunnerPlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _steerSpeed = 5f;
        [SerializeField] private float _responsiveness = 10f;
        [SerializeField] private float _clampX = 3f;

        private Transform _playerFollower;
        private bool _active;
        private const float StaggerDuration = 0.5f;

        private float _currentMoveSpeed;
        private Tween _moveSpeedTween;
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                //_anim.SetBool(_runningParam, _active);
            }
        }
        
        private bool _started;
        private float _offsetX;
        //private Animator _anim;
        private RunnerPlayerBehaviour _player;
        private float _staggerTimer;
        
        //private static readonly int _offsetDeltaParam = Animator.StringToHash("OffsetDelta");
        //private static readonly int _runningParam = Animator.StringToHash("Running");

        private void Start()
        {
            _player = GetComponent<RunnerPlayerBehaviour>();
            _playerFollower = CameraController.Instance.PlayerFollower;
            //_anim = GetComponentInChildren<Animator>();
            _started = false;
            Subscribe();
            Active = true;
        }
    
        private void OnDestroy()
        {
            Unsubscribe();
        }
    
        private void Update()
        {
            if (!Active)
                return;

            if (_staggerTimer > 0)
            {
                _staggerTimer -= Time.deltaTime;
                return;
            }
            
            var pos = transform.position;
            pos.z += _currentMoveSpeed * Time.deltaTime;
            pos.x = Mathf.Lerp(pos.x, _offsetX, _responsiveness * Time.deltaTime);
            transform.position = pos;
            _playerFollower.position = new Vector3(0f, pos.y, pos.z);
            //_anim.SetFloat(_offsetDeltaParam, _offsetX - pos.x);
        }

        private void Subscribe()
        {
            InputController.Instance.Pressed += OnPressed;
            InputController.Instance.Released += OnRelease;
            InputController.Instance.Moved += OnMoved;
            LunchBox.ObstacleHit += Stagger;
            LiquidPlate.Filling += Stagger;
        }

        private void Unsubscribe()
        {
            if (!InputController.Instance)
                return;
            
            InputController.Instance.Pressed -= OnPressed;
            InputController.Instance.Released -= OnRelease;
            InputController.Instance.Moved -= OnMoved;
            LunchBox.ObstacleHit -= Stagger;
            LiquidPlate.Filling -= Stagger;
        }
        
        private void OnPressed(Vector3 pos)
        {
            _started = true;
            UIController.Instance.ToggleTutorialPanel(false);
            _moveSpeedTween.Kill();
            _moveSpeedTween = DOTween.To(() => _currentMoveSpeed, x => _currentMoveSpeed = x, _moveSpeed, 0.2f);
        }
        
        private void OnRelease(Vector3 obj)
        {
            _moveSpeedTween.Kill();
            _moveSpeedTween = DOTween.To(() => _currentMoveSpeed, x => _currentMoveSpeed = x, 0f, 0.2f);
        }
    
        private void OnMoved(Vector3 inputDelta)
        {
            _offsetX = Mathf.Clamp(_offsetX + inputDelta.x * _steerSpeed, -_clampX, _clampX);
        }

        private void Stagger()
        {
            _staggerTimer = StaggerDuration;
        }
    }
}
