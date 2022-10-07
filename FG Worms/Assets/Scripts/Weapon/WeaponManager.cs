using Seb.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Seb.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        // Serialized fields, editable in unity inspector (See [Tooltip()] for what they do)
        [Header("Weapon manager settings")]
        [SerializeField]
        [Tooltip("Weapons the player starts with")]
        private List<GameObject> _startingWeapons;

        [SerializeField]
        [Tooltip("Enables looping through weapons")]
        private bool _enableLoopScroll = true;

        [SerializeField]
        [Tooltip("GameObject that weapons will be appended to")]
        private GameObject _weaponParent;

        // Private declared variables
        private int _activeWeapon;
        private bool _enableSwitch = true;

        private void Awake()
        {
            foreach (var weapon in _startingWeapons)
            {
                var spawned = Instantiate(weapon, _weaponParent.transform);
                spawned.SetActive(false);
            }

            for (int i = 0; i < _weaponParent.transform.childCount;)
            {
                _activeWeapon = i;
                _weaponParent.transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }

        void Update()
        {
            if (!_enableSwitch)
            {
                InputManager.switchWeaponInput = 0;
                return;
            } 

            if (InputManager.switchWeaponInput != 0f)
            {
                int newWeapon = InputManager.switchWeaponInput;

                if (newWeapon >= 1 && newWeapon <= 9) // Keyboard numbers 
                {
                    DoSwitchWeapon(newWeapon - 1);
                }
                else if (newWeapon == 120) // Scroll or Gamepad positive
                {
                    DoSwitchWeapon(_activeWeapon - 1);
                }
                else if (newWeapon == -120) // Scroll or Gamepad negative
                {
                    DoSwitchWeapon(_activeWeapon + 1);
                }
                InputManager.switchWeaponInput = 0;
            }
        }

        public bool SetEnableSwitch
        {
            get { return _enableSwitch; }
            set { _enableSwitch = value; }
        }

        private void DoSwitchWeapon(int nextWeapon = 0)
        {
            int weaponCount = _weaponParent.transform.childCount; // number of weapons
            if (weaponCount == 0) return;

            void Switch(int newIndex) // Switches weapon
            {
                _weaponParent.transform.GetChild(_activeWeapon).gameObject.SetActive(false);
                _weaponParent.transform.GetChild(newIndex).gameObject.SetActive(true);
            }

            if (nextWeapon != _activeWeapon && nextWeapon < weaponCount && nextWeapon >= 0) // newIndex = nextWeapon
            {
                Switch(nextWeapon);
                _activeWeapon = nextWeapon;
            }
            else if (nextWeapon == weaponCount && _enableLoopScroll) // Loops through weapons. newIndex = 0. First weapon
            {
                Switch(0);
                _activeWeapon = 0;
            }
            else if (nextWeapon < 0 && _enableLoopScroll) // newIndex = amounts of weapons - 1. Last weapon 
            {
                Switch(weaponCount - 1);
                _activeWeapon = weaponCount - 1;
            }
        }
    }
}
