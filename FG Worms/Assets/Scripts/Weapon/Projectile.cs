using System.Collections;
using UnityEngine;

namespace Seb.Weapon
{
    [RequireComponent(typeof(Rigidbody))] // Requires a CharacterController (unity will add one if not present)
    public class Projectile : MonoBehaviour
    {
        private enum ProjectileType { OnTriggerEnter, TimeDelay };
        private enum OnTriggerEnterBehaviour { DamageHitCollider, Explode };

        // Serialized fields, editable in unity inspector (See [Tooltip()] for what they do)
        [Header("Settings")]
        [SerializeField]
        [Tooltip("Type of projectile")]
        private ProjectileType _projectileType;

        [SerializeField]
        [Tooltip("Projectile damage")]
        private float _damage = 1;

        [SerializeField]
        [Tooltip("Projectile speed")]
        private float _projectileSpeed = 10f;

        [SerializeField]
        [Tooltip("Strength of force applied to hit object")]
        private float _forceStrength = 1f;

        [SerializeField]
        [Tooltip("Angle for the cone in which the bullets will be shot randomly (0 means no spread at all)")]
        private float _bulletSpreadAngle = 0f;

        [SerializeField]
        [Tooltip("Time in seconds untill the projectile gets destroyed or explodes")]
        private float _time = 5f;

        [SerializeField]
        [Tooltip("GameObject(VFX) to instantiate on impact or explosion")]
        private GameObject _gameObjectToSpawn;

        [Header("On Trigger Enter Only")]
        [SerializeField]
        [Tooltip("Range of explosion")]
        private OnTriggerEnterBehaviour _onTriggerEnterBehaviour;

        [Header("Explode Only")]
        [SerializeField]
        [Tooltip("Range of explosion")]
        private float _range = 5f;

        private bool _damageOnTriggerEnter = false;

        // Setup from weapon controller 
        public void Setup(Vector3 shootDir)
        {
            transform.forward = shootDir; // Sets rotation of projectile to face shoot direction
            Vector3 finalShootDir = Vector3.Slerp(shootDir, Random.insideUnitSphere, _bulletSpreadAngle / 180f); // Sets shoot direction with option for cone spread

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(finalShootDir * _projectileSpeed, ForceMode.Impulse); // Moves projectile

            switch (_projectileType)
            {
                case ProjectileType.OnTriggerEnter:
                    _damageOnTriggerEnter = true;
                    Destroy(gameObject, _time);
                    break;
                case ProjectileType.TimeDelay:
                    StartCoroutine(WaitThenExplode());
                    break;
                default:
                    break;
            }
        }

        private IEnumerator WaitThenExplode()
        {
            yield return new WaitForSeconds(_time);
            DoExplode();
            yield return null;
        }

        private void DoDamageOther(Collider other)
        {
            HealthScript health = other.gameObject.GetComponent<HealthScript>(); // Check if the object we hit has a health script attached
            if (health != null) // If there was a health script attached
            {
                // Call the damage function of that script, passing in our gunDamage variable
                health.Damage(_damage);
            }

            // Add force to hit object
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody != null)
            {
                Vector3 dir = transform.position - other.transform.position;
                dir.Normalize();
                other.attachedRigidbody.AddForce(dir * _forceStrength, ForceMode.Impulse);
            }

            if (_gameObjectToSpawn) Instantiate(_gameObjectToSpawn, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void DoExplode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _range);

            foreach (Collider collider in colliders)
            {
                HealthScript health = collider.gameObject.GetComponent<HealthScript>();
                if (health != null) // If there was a health script attached
                {
                    // Call the damage function of that script, passing in our gunDamage variable
                    health.Damage(_damage);
                }
            }

            if (_gameObjectToSpawn) Instantiate(_gameObjectToSpawn, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_damageOnTriggerEnter) return;

            switch (_onTriggerEnterBehaviour)
            {
                case OnTriggerEnterBehaviour.DamageHitCollider:
                    DoDamageOther(other);
                    break;
                case OnTriggerEnterBehaviour.Explode:
                    DoExplode();
                    break;
            }
        }
    }
}
