using UnityEngine;

public class SpawnCollectible : MonoBehaviour
{
    [SerializeField]
    private GameObject _collectiblePrefab;

    [SerializeField]
    private float _spawnHeight = 0f;

    [SerializeField]
    private Vector2 _spawnSize = Vector2.zero;

    [SerializeField]
    private int _timesToTry = 2;

    [SerializeField]
    [Range(0f, 1f)]
    private float _chanceToSpawn = 0.5f;


    public void DoSpawnHealth()
    {
        if (_collectiblePrefab == null) return;

        for (int i = 0; i < _timesToTry; i++)
        {
            if (Random.value > _chanceToSpawn)
            {
                Instantiate(_collectiblePrefab, transform.position + RandomPoint(), Quaternion.identity);
            }
        }
    }

    private Vector3 RandomPoint()
    {
        return new Vector3(
            Random.Range(-_spawnSize.x, _spawnSize.x) / 2,
            _spawnHeight,
            Random.Range(-_spawnSize.y, _spawnSize.y) / 2
            );
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(_spawnSize.x ,0f ,_spawnSize.y));
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, _spawnHeight, 0f), new Vector3(_spawnSize.x, 0f, _spawnSize.y));
    }
}
