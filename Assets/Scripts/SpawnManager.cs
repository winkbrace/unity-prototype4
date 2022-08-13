using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    private GameObject player;
    private const float spawnRange = 9.0f;
    public int enemyCount;
    public int waveNumber = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        spawnNewWave();
    }

    private void spawnNewWave()
    {
        for (var i = 0; i < waveNumber; i++)
            Instantiate(enemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
        
        Instantiate(powerupPrefab, GenerateRandomPosition(0.5f), powerupPrefab.transform.rotation);
    }

    private Vector3 GenerateRandomPosition(float y = 0)
    {
        var playerPos = player.transform.position;
        var randomPos = playerPos;

        // Ensure they don't spawn too close to the player.
        while (Math.Abs(randomPos.x - playerPos.x) < 2 && Math.Abs(randomPos.z - playerPos.z) < 2) {
            randomPos = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                y,
                Random.Range(-spawnRange, spawnRange)
            );
        }

        return randomPos;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0) {
            waveNumber++;
            spawnNewWave();
        }
    }
}
