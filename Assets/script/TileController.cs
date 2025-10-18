using UnityEngine;

public class TileController : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] float destroyZ = -25f;
    [SerializeField] float spawnTriggerZ = -15f;
    [SerializeField] GameObject nextTilePrefab;
    [SerializeField] Transform spawnPoint;

    bool hasSpawnedNext = false;

    void Update()
    {
        // Gerakkan tile mundur
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        // Spawn tile berikutnya kalau sudah cukup jauh
        if (!hasSpawnedNext && transform.position.z <= spawnTriggerZ)
        {
            SpawnNextTile();
            hasSpawnedNext = true;
        }

        // Hapus tile lama
        if (transform.position.z <= destroyZ)
        {
            Destroy(gameObject);
        }
    }

    void SpawnNextTile()
    {
        if (nextTilePrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Next Tile Prefab atau Spawn Point belum diassign!");
            return;
        }

        // Pastikan Y tetap sama agar tile tidak turun
        Vector3 pos = spawnPoint.position;
        pos.y = transform.position.y;

        Instantiate(nextTilePrefab, pos, Quaternion.identity);
    }
}
