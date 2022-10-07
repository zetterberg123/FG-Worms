using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Seb.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private bool _enableOnAwake = false;

        [SerializeField]
        private TextMeshPro _playerText;

        [SerializeField]
        private UnityEvent _enableMovment;

        [SerializeField]
        private UnityEvent _disableMovment;

        [SerializeField]
        private UnityEvent _enableWeapon;

        [SerializeField]
        private UnityEvent _disableWeapon;

        [SerializeField]
        private UnityEvent _enableWeaponSwitch;

        [SerializeField]
        private UnityEvent _disableWeaponSwitch;

        [SerializeField]
        private UnityEvent _onDestroy;

        private GameManager _gameManager;
        private bool _active;

        private void Awake()
        {
            if (_enableOnAwake)
            {
                _enableMovment.Invoke();
                _enableWeapon.Invoke();
                _enableWeaponSwitch.Invoke();
            }
            else
            {
                _disableMovment.Invoke();
                _disableWeapon.Invoke();
                _disableWeaponSwitch.Invoke();
            }
        }

        private void OnDestroy()
        {
            _onDestroy.Invoke();
        }

        public void SetupPlayer(GameManager gameManager, Color32 teamColor)
        {
            _gameManager = gameManager;

            if (_playerText) _playerText.color = teamColor;
        }

        public void EnablePlayer()
        {
            _active = true;
            _enableMovment.Invoke();
            _enableWeapon.Invoke();
            _enableWeaponSwitch.Invoke();
        }

        public void DisablePlayer()
        {
            _active = false;
            _disableMovment.Invoke();
            _disableWeapon.Invoke();
            _disableWeaponSwitch.Invoke();
        }

        public void EnableWeapon()
        {
            _enableWeapon.Invoke();
        }

        public void DisableWeapon()
        {
            _disableWeapon.Invoke();
        }

        public void EnableWeaponSwitch()
        { 
            _enableWeaponSwitch.Invoke();
        }

        public void DisableWeaponSwitch()
        {
            _disableWeaponSwitch.Invoke();
        }

        public void EndTurn()
        {
            if (_active && _gameManager) _gameManager.UpdateGameState(GameManager.GameState.EndTurn);
        }
        
        public void WaitEndTurn(float waitTime)
        {
            StartCoroutine(DoWaitEndTurn(waitTime));
        }

        public void DestroyPlayer()
        {
            if (_active && _gameManager) _gameManager.UpdateGameState(GameManager.GameState.EndTurn);
            gameObject.SetActive(false);
            Destroy(gameObject, 4);
        }

        private IEnumerator DoWaitEndTurn(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            EndTurn();
        }
    }
}
