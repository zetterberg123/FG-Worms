using UnityEngine;

namespace Seb.Collectibles
{
    public class HPCollectible : MonoBehaviour, ICollectible
    {
        [SerializeField]
        private float _health = 1f;

        public void Collect(GameObject other = null)
        {
            if (other != null)
            {
                HealthScript healthScript = other.GetComponent<HealthScript>();
                if (healthScript.currentHealth >= healthScript.maxHealth) return;
                else
                {
                    healthScript.Heal(healFloat: _health);
                    Destroy(gameObject);
                }
            }
        }
    }
}
