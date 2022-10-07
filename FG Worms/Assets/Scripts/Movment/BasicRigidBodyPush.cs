using UnityEngine;

namespace Seb.Movement
{
    // Not coded by me. Example code taken from documentation
    public class BasicRigidBodyPush : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _pushLayers;

        [SerializeField]
        private bool _canPush;

        [SerializeField]
        [Range(0.5f, 5f)]
        private float _strength = 1f;

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_canPush) PushRigidBodies(hit);
        }

        private void PushRigidBodies(ControllerColliderHit hit)
        {
            // https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

            // make sure we hit a non kinematic rigidbody
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic) return;

            // make sure we only push desired layer(s)
            var bodyLayerMask = 1 << body.gameObject.layer;
            if ((bodyLayerMask & _pushLayers.value) == 0) return;

            // We dont want to push objects below us
            if (hit.moveDirection.y < -0.3f) return;

            // Calculate push direction from move direction, horizontal motion only
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

            // Apply the push and take strength into account
            body.AddForce(pushDir * _strength, ForceMode.Impulse);
        }
    }
}