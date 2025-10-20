using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollectibles : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject kertasPrefab;
    public Transform[] spawnPoints; // titik di mana kertas bisa muncul

    void Start()
    {
        SpawnPapers();
    }

    void SpawnPapers()
    {
        foreach (Transform point in spawnPoints)
        {
            if (Random.value > 0.5f) // 50% kemungkinan muncul
            {
                Instantiate(kertasPrefab, point.position, Quaternion.identity, transform);
            }
        }
    }
}
