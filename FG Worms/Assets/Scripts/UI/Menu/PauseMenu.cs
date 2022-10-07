using Seb.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Seb.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onPause;

        [SerializeField]
        private UnityEvent _onResume;

        private bool _gameIsPaused = false;

        void Update()
        {
            if (InputManager.pauseInput)
            {
                InputManager.pauseInput = false;
                if (_gameIsPaused) Resume();
                else Pause();
            }
        }

        public void Pause()
        {
            _onPause.Invoke();
            Time.timeScale = 0f;
            _gameIsPaused = true;
        }

        public void Resume()
        {
            _onResume.Invoke();
            Time.timeScale = 1f;
            _gameIsPaused = false;
        }
    }
}
