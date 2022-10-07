using UnityEngine;
using UnityEngine.VFX;

namespace Seb.Other
{
    public class SpawnMeshDustVFX : MonoBehaviour
    {
        [SerializeField]
        private GameObject _vfxPrefab;

        [SerializeField]
        private GameObject _meshGameObject;

        void Awake()
        {
            if (!_meshGameObject)
            {
                Debug.LogError("No meshGameObject assigned");
                Destroy(this);
            }
        }

        public void DoSpawnVFX()
        {
            GameObject vfxPrefab = Instantiate(_vfxPrefab, _meshGameObject.transform.position, _meshGameObject.transform.rotation);
            vfxPrefab.TryGetComponent(out VisualEffect visualEffect);

            _meshGameObject.TryGetComponent(out MeshFilter mesh);

            if (visualEffect && mesh)
            {
                visualEffect.SetMesh("Mesh", mesh.sharedMesh);
                visualEffect.Play();
                Destroy(vfxPrefab, 5);
            }
            else
            {
                print("Error! No visualEffect: " + visualEffect + " or mesh: " + mesh);
                Destroy(vfxPrefab);
            }
        }
    }
}
