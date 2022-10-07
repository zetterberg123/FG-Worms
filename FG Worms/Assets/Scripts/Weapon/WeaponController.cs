using Seb.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Seb.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        #region variables
        private enum GunType { Raycast, Rigidbody };
        private enum ShootType { Semi, Auto };
        private enum ProjectileShootDir { TransformForward, CenterOfScreen };

        // Serialized fields, editable in unity inspector (See [Tooltip()] for what they do)
        [Header("Shooting system settings")]
        [SerializeField]
        [Tooltip("The type of shooting system the weapon will use")]
        private GunType _gunType;

        [SerializeField]
        [Tooltip("The shooting type the weapon will use")]
        private ShootType _shootType;

        [Header("Settings")]
        [SerializeField]
        [Tooltip("Number in seconds which controls how often the player can fire")]
        private float _fireRate = 0.25f;

        [Header("Raycast settings")]
        [SerializeField]
        [Tooltip("Set the number of hitpoints that this gun will take away from shot objects with a health script")]
        private float _gunDamage = 1;

        [SerializeField]
        [Tooltip("Distance in Unity units over which the player can fire")]
        private float _weaponRange = 50f;

        [SerializeField]
        [Tooltip("Amount of force which will be added to objects with a rigidbody shot by the player")]
        private float _hitForce = 100f;

        [Header("Projectile settings")]
        [SerializeField]
        [Tooltip("Target for projectile")]
        private ProjectileShootDir _projectileShootDir;

        [SerializeField]
        [Tooltip("Hold a refrence to projectile prefab")]
        private Transform _projectilePrefab;

        [SerializeField]
        [Tooltip("Amount of bullets per shot")]
        private int _bulletsPerShot = 1;

        [Header("Internal References")]
        [SerializeField]
        [Tooltip("Holds a reference to the gun end object, marking the muzzle location of the gun")]
        private Transform _gunEnd;

        [Header("Event")]
        [SerializeField]
        private UnityEvent _onShoot;

        // Private declared variables
        private float _nextFire; // Next time you can fire
        #endregion

        #region Unity callbacks
        void Update()
        {
            if (InputManager.shootInput && Time.time > _nextFire) // If shoot button is pressed and time is greater then nextFire
            {
                _nextFire = Time.time + _fireRate; // Resets nextFire time delay
                _onShoot.Invoke();

                for (int i = 0; i < _bulletsPerShot; i++)
                {
                    switch (_shootType)
                    {
                        case ShootType.Semi:
                            InputManager.shootInput = false;
                            TryShoot();
                            break;
                        case ShootType.Auto:
                            TryShoot();
                            break;
                    }
                }
            }
        }
        #endregion

        private void TryShoot()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Create a ray from middle of screen
            
            switch (_gunType) // Select shooting system 
            {
                case GunType.Raycast:
                    ShootRaycast(ray);
                    break;
                case GunType.Rigidbody:
                    ShootRigidbody(ray);
                    break;
                default:
                    break;
            }

        }
        private void ShootRaycast(Ray ray)
        {
            RaycastHit hit; // Holds a reference to hit object

            if (Physics.Raycast(ray, out hit, _weaponRange)) // If ray hit
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);

                // Check if the object we hit has a health script attached
                HealthScript health = hit.collider.GetComponent<HealthScript>();

                // If there was a health script attached
                if (health != null)
                {
                    // Call the damage function of that script, passing in our gunDamage variable
                    health.Damage(_gunDamage);
                }

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce(-hit.normal * _hitForce);
                }
            }
        }

        private void ShootRigidbody(Ray ray)
        {
            RaycastHit hit; // Holds a reference to hit object
            Vector3 shootDir = Vector3.zero; // Final shoot target

            switch (_projectileShootDir)
            {
                case ProjectileShootDir.TransformForward:
                    shootDir = _gunEnd.forward;
                    break;
                case ProjectileShootDir.CenterOfScreen:
                    if (Physics.Raycast(ray, out hit)) // if ray hit
                    {
                        shootDir = hit.point; // shoot at hit point
                        Debug.DrawLine(ray.origin, shootDir, Color.red);
                    }
                    else
                    {
                        shootDir = ray.GetPoint(1000f); // else shoot at a point in the middle of the screen 1000 units away
                        Debug.DrawLine(ray.origin, shootDir, Color.green);
                    }
                    shootDir = (shootDir - _gunEnd.position).normalized;
                    break;
            }

            if (_projectilePrefab != null) // Check if projectilePrefab is assigned. If true spawns Projectile
            {
                Transform projectileTransform = Instantiate(_projectilePrefab, _gunEnd.position, Quaternion.identity);
                projectileTransform.GetComponent<Projectile>().Setup(shootDir);
            }
            else Debug.LogError("No projectile prefab assigned");
        }
    }

}