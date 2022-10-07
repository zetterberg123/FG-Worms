using UnityEngine;

namespace Seb.Collectibles
{
    public interface ICollectible
    {
        public void Collect(GameObject other = default);
    }
}
