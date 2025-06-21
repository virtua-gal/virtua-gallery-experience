using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemToSpawn;
    [SerializeField] private float spawnRate = 2.5f;
    [SerializeField] private float minx = -10;
    [SerializeField] private float maxx = 10;
    [SerializeField] private float minz = -10;
    [SerializeField] private float maxz = 10;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnItem", 0, spawnRate);
    }

    void SpawnItem()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x + Random.Range(minx, maxx), 
        transform.position.y, transform.position.z + Random.Range(minz, maxz));

        Instantiate(itemToSpawn, spawnPoint, Quaternion.identity);
    }
}
