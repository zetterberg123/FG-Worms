using UnityEngine;

namespace Seb.Other
{
    public class DamageBox : MonoBehaviour
    {
        [SerializeField]
        private float _damage = 1;

        private HealthScript _healthScript;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out _healthScript))
            {
                _healthScript.Damage(_damage);
            }
        }
    }
}
