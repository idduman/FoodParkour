using UnityEngine;

namespace HyperCore.Runner
{
    [RequireComponent(typeof(RunnerPlayerController))]
    public class RunnerPlayerBehaviour : MonoBehaviour
    {
        public static float ZCoordinate;
        
        private Transform _finish;

        private RunnerPlayerController _controller;

        private bool _finished;

        void Start()
        {
            ZCoordinate = transform.position.z;
            _controller = GetComponent<RunnerPlayerController>();
            _finish = GameObject.FindGameObjectWithTag("Finish").transform;
            if(!_finish)
                Debug.LogError("No finish line found in level!");
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        void Update()
        {
            if (_finished)
                return;

            ZCoordinate = transform.position.z;

            if (transform.position.z > _finish.position.z)
                Finish(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IObstacle>(out var obs))
            {
                Finish(false);
            }
            else if (other.TryGetComponent<ICollectible>(out var col))
            {
                col.Pickup();
            }
        }

        private void Finish(bool success)
        {
            if (_finished)
                return;

            _finished = true;
            _controller.OnFinish();
            StopAllCoroutines();
            GameManager.Instance.FinishGame(success);
        }
    }
}