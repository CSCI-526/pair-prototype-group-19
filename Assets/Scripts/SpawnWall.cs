using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] wallPrefabs;
    public float spawnBegin = 2.0f;
    public float spawnInterval = 2.0f;
    public float secondWallSpawnProbability = 0.33f;

    // Start is called before the first frame update
    void Start()
    {
        // Starting from spawnBegin seconds, spawn wall every spawnInterval seconds.
        InvokeRepeating("SpawnWall", spawnBegin, spawnInterval);
    }

    void SpawnWall()
    {
        // Spawn the first wall
        int firstWallIndex = Random.Range(0, wallPrefabs.Length);
        Instantiate(wallPrefabs[firstWallIndex], wallPrefabs[firstWallIndex].transform.position, wallPrefabs[firstWallIndex].transform.rotation);

        // Check if we should spawn a second wall
        if (Random.Range(0f, 1f) < secondWallSpawnProbability)
        {
            int secondWallIndex = Random.Range(0, wallPrefabs.Length);

            // Ensure the second wall is not the same as the first one
            while (secondWallIndex == firstWallIndex)
            {
                secondWallIndex = Random.Range(0, wallPrefabs.Length);
            }

            // Spawn the second wall
            Instantiate(wallPrefabs[secondWallIndex], wallPrefabs[secondWallIndex].transform.position, wallPrefabs[secondWallIndex].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Add any update logic here if needed
    }
}
