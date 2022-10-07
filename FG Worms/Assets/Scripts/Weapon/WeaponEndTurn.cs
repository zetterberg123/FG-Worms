using Seb.Managers;
using UnityEngine;

public class WeaponEndTurn : MonoBehaviour
{
    [SerializeField]
    private float _endTurnDelay = 0f;

    [SerializeField]
    private int _endTurnSkip = 0;

    [SerializeField]
    private bool _disableWeaponSwitch = true;

    private PlayerManager _playerManager;
    private int _skip;

    private void Awake()
    {
        _playerManager = GetComponentInParent<PlayerManager>();
        if (_playerManager == null) Destroy(this);
        _skip = _endTurnSkip;
    }

    private void OnEnable()
    {
        _skip = _endTurnSkip;
    }

    public void EndTurn()
    {
        if (_skip <= 0) DoEndTurn();
        _skip--;

        if (_disableWeaponSwitch)
        {
            _disableWeaponSwitch = false;
            _playerManager.DisableWeaponSwitch();
        }
    }

    private void DoEndTurn()
    {
        _playerManager.DisableWeapon();
        _playerManager.WaitEndTurn(_endTurnDelay);
    }
}
