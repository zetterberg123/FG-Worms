using UnityEngine;

namespace Seb.Collectibles
{
    public class Collector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            ICollectible collectible = other.GetComponent<ICollectible>();
            if (collectible != null)
            {
                collectible.Collect(gameObject);
            }
        }
    }
}
