using UnityEngine;

public class EnvironmentMover : MonoBehaviour
{
    public GameObject[] tiles;  // Masukkan Tile_01, Tile_02, dst.
    public float speed = 10f;
    public float resetZ = -20f;
    public float startZ = 40f;

    void Update()
    {
        foreach (GameObject tile in tiles)
        {
            // Gerakkan tile ke belakang
            tile.transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

            // Reset posisi tile yang sudah lewat kamera
            if (tile.transform.position.z <= resetZ)
            {
                Vector3 newPos = tile.transform.position;
                newPos.z = startZ;
                tile.transform.position = newPos;
            }
        }
    }
}

