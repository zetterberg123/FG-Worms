using UnityEngine;
using Cinemachine;
using UnityEngine.VFX;
using System.Collections;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Seb.Other
{
    [RequireComponent(typeof(VisualEffect))]
    public class ExplosionVFX : MonoBehaviour
    {
        private VisualEffect _visualEffect;

        void Awake()
        {
            TryGetComponent(out _visualEffect);
            TryGetComponent(out CinemachineImpulseSource impulseSource);

            if (impulseSource) impulseSource.GenerateImpulse();
            if (_visualEffect) StartCoroutine(DisposeVFX());
        }

        IEnumerator DisposeVFX()
        {
            yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => _visualEffect.aliveParticleCount == 0);

            Destroy(gameObject);
            yield break;
        }

    }
}
