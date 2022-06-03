using HyperCore;
using UnityEngine;

namespace Dixy.FoodParkour
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private Transform _finish;
        private bool _finished;

        void Start()
        {
            _finish = GameObject.FindGameObjectWithTag("Finish").transform;
            if(!_finish)
                Debug.LogError("No finish line found in level!");
            
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
        }

        private void OnPressed(Vector3 obj)
        {
            UIController.Instance.ToggleTutorialPanel(false);
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
    }
}