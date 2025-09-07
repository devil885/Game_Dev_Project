using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave 
    {
        public string waveName;
        public int waveSize;    //total number of enemies to spawn
        public float waveSpawnRate;  // how often to spawn enemies
        public int spawnedCount;  // how many enemies are spawned already
        public List<EnemyGroup> enemyGroups;
    }

    [System.Serializable]
    public class EnemyGroup 
    {
        public string enemyName;
        public int enemyCount; //number of enemies to spawn
        public int spawnedCount; //number of spawned enemies
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveIndex; // index of current wave
    Transform player;

    [Header("Spawner Attributes")]
    float spawnTimer;
    public float waveInterval;
    public int enemiesAlive;
    public int maxEnemyCount;// max number of alive enemies at once
    public bool maxEnemiesReached = false;
    bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints;

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>().transform;
        CalculateWaveSize();
    }

    
    void Update()
    {
        if (currentWaveIndex < waves.Count && waves[currentWaveIndex].spawnedCount == 0 && !isWaveActive) 
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveIndex].waveSpawnRate) 
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave() 
    {
        isWaveActive = true;
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveIndex < waves.Count - 1) 
        {
            isWaveActive = false;
            currentWaveIndex++; 
            CalculateWaveSize();
        }
    }

    void CalculateWaveSize() 
    {
        int currentWaveSize = 0;
        foreach (var enemyGroup in waves[currentWaveIndex].enemyGroups) 
        {
            currentWaveSize += enemyGroup.enemyCount;
        }

        waves[currentWaveIndex].waveSize = currentWaveSize;
    }

    void SpawnEnemies() 
    {
        if (waves[currentWaveIndex].spawnedCount < waves[currentWaveIndex].waveSize && !maxEnemiesReached) 
        {
            foreach(var enemyGroup in waves[currentWaveIndex].enemyGroups) 
            {
                if (enemyGroup.spawnedCount < enemyGroup.enemyCount) 
                {
                    Instantiate(enemyGroup.enemyPrefab,player.position + relativeSpawnPoints[UnityEngine.Random.Range(0,relativeSpawnPoints.Count)].position,Quaternion.identity);

                    enemyGroup.spawnedCount++;
                    waves[currentWaveIndex].spawnedCount++;
                    enemiesAlive++;

                    if (enemiesAlive >= maxEnemyCount)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
    }

    public void OnEnemyKilled() 
    {
        enemiesAlive--;

        if (enemiesAlive < maxEnemyCount)
        {
            maxEnemiesReached = false;
        }
    }
}
