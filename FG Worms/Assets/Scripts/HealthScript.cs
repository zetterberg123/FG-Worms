using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Seb
{
    public class HealthScript : MonoBehaviour
    {
        private enum OnDeath { nothing, disable, destroy };

        [SerializeField]
        [Tooltip("Game object start health")]
        private float startHealth = 1;

        [Tooltip("Game object max health (same as start health if 0)")]
        public float maxHealth;

        [SerializeField]
        [Tooltip("What to do when game object dies")]
        private OnDeath onDeath = OnDeath.destroy;

        [SerializeField]
        [Tooltip("Put player health at 1 if damage taken will one shoot and player health is above 50%")]
        private bool oneHitProtection = false;

        [SerializeField]
        [Tooltip("Unity event called on damage")]
        private UnityEvent onDamageEvent;

        [SerializeField]
        [Tooltip("Unity event called on death")]
        private UnityEvent onDeathEvent;

        [SerializeField]
        [Tooltip("optional text mesh pro text")]
        private TextMeshPro _text;

        [HideInInspector] public float currentHealth;
        [HideInInspector] public bool dead;

        private void Start()
        {
            currentHealth = startHealth;
            if (maxHealth == 0)
                maxHealth = startHealth;

            if (_text) _text.text = currentHealth.ToString();
        }

        public void Damage(float damageAmount) //Called when object is hit by a shooting script
        {
            onDamageEvent.Invoke();

            if (oneHitProtection && damageAmount > currentHealth && currentHealth < startHealth / 2) // One Hit Protection
                currentHealth = 1f;
            else
                currentHealth -= damageAmount; // Removes health from object

            if (currentHealth <= 0 && !dead)
            {
                // Invokes onDeath events
                onDeathEvent.Invoke();
                dead = true;

                switch (onDeath)
                {
                    case OnDeath.nothing:
                        break;
                    case OnDeath.disable:
                        gameObject.SetActive(false);
                        break;
                    case OnDeath.destroy:
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }

            if (_text) _text.text = currentHealth.ToString();
        }

        public void Heal(float healFloat = 0f, float healPercentage = 0f, bool healFull = false)
        {
            if (healFull) // healFull
                currentHealth = maxHealth;
            else if (healPercentage > 0 && healPercentage < 100) // healPercentage
            {
                if (healPercentage < 1)
                    currentHealth = (maxHealth * healPercentage);
                else if (healPercentage < 100)
                    currentHealth = (maxHealth * healPercentage) / 100;
            }
            else // healFloat
                currentHealth += healFloat;

            if (currentHealth > maxHealth) // Don't heal over maxHealth
                currentHealth = maxHealth;

            if (dead && currentHealth > 0)
                dead = false;

            if (_text) _text.text = currentHealth.ToString();
        }
    }
}
