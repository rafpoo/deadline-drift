using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject[] tilePrefabsLevel2;
    [SerializeField] private GameObject[] tilePrefabsLevel3;
    [SerializeField] private int numberOfTiles = 10;
    [SerializeField] private float tileLength = 49.48f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private float playerYOffset = 1f;
    [SerializeField] private float playerZOffset = -5f;
    [SerializeField] private float playerXOffset = 0.29f;
    [SerializeField] private float recycleDistanceMultiplier = 1.5f;
    [SerializeField] private Transform playerSpawnPoint;

    private List<GameObject> activeTiles = new List<GameObject>();
    private Animator anim;

    private bool isMoving = true;
    private float baseMoveSpeed;

    public static TileManager Instance { get; private set; }

    void Awake()
    {
        // Singleton (biar mudah diakses dari GameManager)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    IEnumerator Start()
    {
        anim = player.GetComponent<Animator>();
        baseMoveSpeed = moveSpeed;
        // Spawn semua tile dulu
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile(i < 2 ? 0 : Random.Range(0, tilePrefabs.Length));
        }

        // Tunggu 1 frame biar semua posisi tile stabil
        yield return null;

        // Baru posisikan player
        // if (player != null)
        // {
        //     if (playerSpawnPoint != null)
        //     {
        //         player.position = playerSpawnPoint.position;
        //     }
        //     else if (activeTiles.Count > 0)
        //     {
        //         Vector3 startPos = activeTiles[0].transform.position;
        //         player.position = new Vector3(
        //             startPos.x + playerXOffset,
        //             startPos.y + playerYOffset,
        //             startPos.z + playerZOffset
        //         );
        //     }
        // }

        if (player != null && playerSpawnPoint != null)
        {
            player.position = playerSpawnPoint.position;
        }


    }

    void Update()
    {
        // Cek GameOver dulu
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            if (isMoving)
                StopTiles();  // set isMoving = false sekali saja

            return;
        }

        // Jika tile tidak bergerak, hentikan Update
        if (!isMoving) return;

        // Gerakkan semua tile
        foreach (var tile in activeTiles)
        {
            tile.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }

        // Recycle tile
        GameObject firstTile = activeTiles[0];
        float distanceFromPlayer = player.position.z - firstTile.transform.position.z;

        if (distanceFromPlayer > tileLength * recycleDistanceMultiplier)
            MoveTileToFront(firstTile);
    }


    void SpawnTile(int prefabIndex)
    {
        float currentY = 0f;
        float newZ = activeTiles.Count > 0
            ? activeTiles[activeTiles.Count - 1].transform.position.z + tileLength
            : 0f;

        Vector3 spawnPos = new Vector3(0, currentY, newZ);
        GameObject tile = Instantiate(tilePrefabs[prefabIndex], spawnPos, Quaternion.identity);
        activeTiles.Add(tile);
    }

    void MoveTileToFront(GameObject tile)
    {
        activeTiles.RemoveAt(0);

        GameObject lastTile = activeTiles[activeTiles.Count - 1];
        float newZ = lastTile.transform.position.z + tileLength;
        float newY = lastTile.transform.position.y;

        tile.transform.position = new Vector3(0, newY, newZ);

        activeTiles.Add(tile);
    }

    // 🔹 Dipanggil saat game over
    public void StopTiles()
    {
        isMoving = false;
        moveSpeed = 0f;
    }

    // 🔹 Dipanggil ulang saat retry
    public void ResetTiles()
    {
        isMoving = true;
        moveSpeed = baseMoveSpeed;
    }
}
