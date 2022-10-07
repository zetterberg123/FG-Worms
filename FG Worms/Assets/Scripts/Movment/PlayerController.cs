using Seb.Managers;
using UnityEngine;

namespace Seb.Movement
{
    [RequireComponent(typeof(CharacterController))] // Requires a CharacterController (unity will add one if not present)
    public class PlayerController : MonoBehaviour
    {
        private enum PlayerState
        {
            Disabled,
            GravityOnly,
            Enabled,
        };

        // Serialized fields, editable in unity inspector (See [Tooltip()] for what they do)
        [Header("Movment Settings")]
        [SerializeField]
        [Tooltip("Move speed of the character in m/s")]
        private float _walkSpeed = 4f;

        [SerializeField]
        [Tooltip("Sprint speed of the character in m/s")]
        private float _sprintSpeed = 6f;

        [Space(10)]
        [SerializeField]
        [Tooltip("The height the player can jump")]
        private float _jumpHeight = 5f;

        [SerializeField]
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        private float _gravity = -9.81f;

        [Header("Mouse Settings")]
        [SerializeField]
        [Tooltip("Reference to player camera. Used to get rotation of movment")]
        private Transform _cameraTransform;

        [SerializeField]
        private float _rotationSpeed = 5f;

        [SerializeField]
        [Tooltip("Locks and hides mouse cursor to screen if checked")]
        private bool _lockCursor;

        [Header("Ground Check Settings")]
        [SerializeField]
        [Tooltip("Ground check empty. Should be located at players feet")]
        private Transform _groundCheckTransform;

        [SerializeField]
        [Tooltip("The radius of the grounded check")]
        private float _groundCheckRadius = 0.2f;

        [SerializeField]
        [Tooltip("What layers the character uses as ground")]
        private LayerMask _groundMask;

        // Private declared variables
        private CharacterController _controller;
        private bool _isGrounded;
        private Vector3 _velocity;
        private PlayerState _state = PlayerState.Enabled;

        public void PlayerStateDisabled()
        {
            _state = PlayerState.Disabled;
        }

        public void PlayerStateGravityOnly()
        {
            _state = PlayerState.GravityOnly;
        }

        public void PlayerStateEnabled()
        {
            _state = PlayerState.Enabled;
        }

        void Start() // Called once before anything else
        {
            _controller = GetComponent<CharacterController>();
            if (!_cameraTransform) _cameraTransform = Camera.main.transform;

            // Locks cursor if True
            if (_lockCursor) Cursor.lockState = CursorLockMode.Locked;
        }

        void Update() // Update is called once per frame
        {
            switch (_state)
            {
                case PlayerState.Disabled:
                    break;
                case PlayerState.GravityOnly:
                    GravityOnly();
                    break;
                case PlayerState.Enabled:
                    JumpAndGravity();
                    Move();
                    break;
                default:
                    break;
            }
        }

        private void JumpAndGravity() // Executes a jump if button is pressed down and player is grounded and apply gravity to player
        {
            _isGrounded = Physics.CheckSphere(_groundCheckTransform.position, _groundCheckRadius, _groundMask, QueryTriggerInteraction.Ignore);

            // Jump
            if (InputManager.jumpInput && _isGrounded)
            {
                // The square root of H * -2 * G = how much velocity needed to reach desired height
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

                InputManager.jumpInput = false; // Sets "jumpInput" to false to disable continuous jumping
            }

            if (_isGrounded && _velocity.y < 0f) // Resets y velocity when grounded
            {
                _velocity.y = -2f;
            }
            _velocity.y += _gravity * Time.deltaTime;
        }

        // Apply gravity to player
        private void GravityOnly()
        {
            _isGrounded = Physics.CheckSphere(_groundCheckTransform.position, _groundCheckRadius, _groundMask, QueryTriggerInteraction.Ignore);

            if (_isGrounded && _velocity.y < 0f) // Resets y velocity when grounded
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        // Moves player according to camera rotation in a frame independent way
        private void Move()
        {
            float targetSpeed = InputManager.sprintInput ? _sprintSpeed : _walkSpeed;

            Vector3 camDir = _cameraTransform.forward; // Normalize rotation so movment speed does not change depending on rotation
            camDir.y = 0;
            camDir.Normalize();

            Vector3 move = _cameraTransform.right * InputManager.moveInput.x + camDir * InputManager.moveInput.y;
            _controller.Move(move * (targetSpeed * Time.deltaTime) + new Vector3(0.0f, _velocity.y, 0.0f) * Time.deltaTime);

            // Rotate player towards camera forward
            Quaternion targetRotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
