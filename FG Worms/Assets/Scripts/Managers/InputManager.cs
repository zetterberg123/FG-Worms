using UnityEngine;
using UnityEngine.InputSystem;

namespace Seb.Managers
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        [Header("Character Input Values")]
        [Header("This script handes handles input for others")]
        public static Vector2 moveInput;
        public static Vector2 cameraInput;
        public static bool jumpInput;
        public static bool sprintInput;
        public static bool pauseInput;
        public static bool shootInput;
        public static int switchWeaponInput;

        void OnMovment(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        void OnCamera(InputValue value)
        {
            cameraInput = value.Get<Vector2>();
        }

        void OnJump(InputValue value)
        {
            jumpInput = value.isPressed;
        }

        void OnSprint(InputValue value)
        {
            sprintInput = value.isPressed;
        }

        void OnShoot(InputValue value)
        {
            shootInput = value.isPressed;
        }

        void OnPause(InputValue value)
        {
            pauseInput = value.isPressed;
        }

        void OnSwitchWeapon(InputValue value)
        {
            switchWeaponInput = Mathf.RoundToInt(value.Get<float>());
        }
    }
}
