using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private int numberOfTiles = 10;
    [SerializeField] private float tileLength = 49.48f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private float playerYOffset = 1f;
    [SerializeField] private float playerZOffset = -5f;
    [SerializeField] private float playerXOffset = 0.29f;
    [SerializeField] private float recycleDistanceMultiplier = 1.5f;


    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnZ = 0f;
    private int tes = 0;

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile(i < 2 ? 0 : Random.Range(0, tilePrefabs.Length));
        }

        if (player != null && activeTiles.Count > 0)
        {
            Vector3 startPos = activeTiles[0].transform.position;
            player.position = new Vector3(
                startPos.x + playerXOffset,
                startPos.y + playerYOffset,
                startPos.z + playerZOffset
            );
        }
    }

    void Update()
    {
        foreach (var tile in activeTiles)
            tile.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

        GameObject firstTile = activeTiles[0];

        // Hitung jarak player ke tile paling belakang
        float distanceFromPlayer = player.position.z - firstTile.transform.position.z;

        // Jika player sudah cukup jauh dari tile belakang → geser tile ke depan
        if (distanceFromPlayer > tileLength * recycleDistanceMultiplier)
        {
            MoveTileToFront(firstTile);
        }

    }


    void SpawnTile(int prefabIndex)
    {
        float currentY = 20f;
        float newZ = activeTiles.Count > 0
            ? activeTiles[activeTiles.Count - 1].transform.position.z + tileLength
            : 0f;

        Vector3 spawnPos = new Vector3(0, currentY, newZ);
        GameObject tile = Instantiate(tilePrefabs[prefabIndex], spawnPos, Quaternion.identity);
        activeTiles.Add(tile);

        Debug.Log("Spawn ke: " + tes);
        tes++;
    }

    void MoveTileToFront(GameObject tile)
    {
        // Hapus dari list depan
        activeTiles.RemoveAt(0);

        // Ambil tile terakhir di list (yang paling depan di dunia)
        GameObject lastTile = activeTiles[activeTiles.Count - 1];

        // Posisi tile baru = posisi tile terakhir + panjang tile
        float newZ = lastTile.transform.position.z + tileLength;
        float newY = lastTile.transform.position.y; // jaga konsistensi ketinggian

        tile.transform.position = new Vector3(0, newY, newZ);

        // Masukkan tile ke belakang list
        activeTiles.Add(tile);
    }

}
