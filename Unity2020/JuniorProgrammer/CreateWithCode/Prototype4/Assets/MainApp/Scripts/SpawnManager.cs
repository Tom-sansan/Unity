using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public int enemiesToSpawn = 1;
    // public int enemyCount;
    private float spawnRange = 9;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(enemiesToSpawn);
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectsOfType<Enemy>().Length == 0)
        {
            enemiesToSpawn++;
            SpawnEnemyWave(enemiesToSpawn);
            if (GameObject.Find("Powerup(Clone)") == null) SpawnPowerup();
        }
    }

    private void SpawnEnemyWave(int _enemiesToSpawn)
    {
        for (int i = 0; i < _enemiesToSpawn; i++)
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
    }

    private void SpawnPowerup() =>
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    private Vector3 GenerateSpawnPosition()
    {
        var spawnPosX = Random.Range(-spawnRange, spawnRange);
        var spawnPosZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnPosX, 0, spawnPosZ);
    }
}
