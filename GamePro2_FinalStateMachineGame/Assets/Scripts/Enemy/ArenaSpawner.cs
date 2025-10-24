using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using UnityEngine;
[System.Serializable]

public class ArenaSpawner : MonoBehaviour
{
    [Header("Wave")]
    public int totalNumberOfWaves;
    public int currentWaveNumber = 0;

    [Header("Enemies")]
    public int totalEnemiesInWave;
    public int enemiesRemaining = 0;
    public int enemiesKilled;
    public int numberOfSpawns = 1;

    public float spawnCooldown = 1.5f;
    private float spawnTime;

    GameObject[] spawners;
    public GameObject[] enemies;

    public activeState spawnMode;
    public enum activeState
    {
        IDLE,
        WAVEACTIVE
    }
    private int loopVar = 0;

    private void Start()
    {
        spawnMode = activeState.IDLE;

        spawners = new GameObject[numberOfSpawns];

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }
        //StartWave();
    }
    private void Update()
    {
        if (enemiesRemaining > 0)
        {
            spawnMode = activeState.WAVEACTIVE;
        }
        else if (enemiesRemaining <= 0)
        {
            spawnMode = activeState.IDLE;
        }
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    private void FixedUpdate()
    {
        switch (spawnMode)
        {
            case activeState.IDLE:
                break;
            case activeState.WAVEACTIVE:
                spawnTime -= Time.deltaTime;
                while (loopVar < totalEnemiesInWave && spawnTime <= 0)
                {
                    spawnCooldown = Random.Range(1, 3);
                    SpawnEnemy();
                    loopVar++;
                    spawnTime = spawnCooldown;
                }
                break;
        }
    }

    private void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);
        GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
        Instantiate(randomEnemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
    }
    private void StartWave()
    {
        loopVar = 0;
        currentWaveNumber++;
        totalEnemiesInWave += Random.Range(2, 5);
        enemiesRemaining = totalEnemiesInWave;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartWave();
        }
    }
}

