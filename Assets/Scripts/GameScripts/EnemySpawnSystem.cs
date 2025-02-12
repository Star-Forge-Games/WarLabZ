using System.Collections;
using UnityEngine;
using static Wave;

public class EnemySpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float randomSpawnInterval;
    [SerializeField] private float spawnZ;
    [SerializeField] private float spawnpointWidth;
    [SerializeField] private GameObject losePanel;
    //[SerializeField] private Transform[] spawnpoints;
    [SerializeField] private Wave[] waves;

    public int wave = 0;
    public int spawnedEnemies = 0, waveEnemies = 0;
    //private int wavePart = 0;
    //private int wavePartCounter = 0;

    /*public float startSpawnerInterval;
    private float spawnerInterval;
    public int numberOfEnemies;
    public int nowTheEnemies;
    public int randEnemy;
    private int randPoint;*/

    void Start()
    {
        // spawnerInterval = startSpawnerInterval;
        CalculateEnemiesAmount();
        ProcessWaveSpawns();
        EnemyZombie.OnZombieHitPlayer += ProcessZombieHit;
    }

    private void CalculateEnemiesAmount()
    {
        waveEnemies = 0;
        foreach (var part in waves[wave].parts)
        {
            waveEnemies += part.amount;
        }
    }

    private void ProcessWaveSpawns()
    {
        foreach (WavePart part in waves[wave].parts)
        {
            StartCoroutine(SpawnEnemies(part, wave == 0));
        }
    }

    private IEnumerator SpawnEnemies(WavePart part, bool first)
    {
        yield return new WaitForSeconds(part.delay + (first ? 0 : waves[wave - 1].nextWaveDelay));
        for (int i = 0; i < part.amount; i++)
        {
            SpawnEnemy(part.zombiePrefab);
            yield return new WaitForSeconds(part.interval);
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        spawnedEnemies++;
        GameObject enemy = Instantiate(prefab, GeneratePoint(), Quaternion.identity);
        enemy.transform.parent = enemyContainer;
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitPlayer -= ProcessZombieHit;
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(waves[wave - 1].nextWaveDelay);
        while (true)
        {
            GameObject enemy = Instantiate(GetRandomEnemy(), GeneratePoint(), Quaternion.identity);
            enemy.transform.parent = enemyContainer;
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }


    private Vector3 GeneratePoint()
    {
        return new Vector3(Random.Range(-spawnpointWidth, spawnpointWidth), 1, spawnZ);
    }

    private GameObject GetRandomEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    private void ProcessZombieHit()
    {
        // Start Death Animation
        StopCoroutine(nameof(SpawnLoop));
        foreach (Transform enemy in enemyContainer)
        {
            enemy.GetComponent<EnemyZombie>().Stop();
        }
        losePanel.SetActive(true);
        // Process other death effects
    }


    void Update()
    {
        if (wave >= waves.Length) return;
        if (spawnedEnemies == waveEnemies)
        {
            spawnedEnemies = 0;
            wave++;
            if (wave >= waves.Length)
            {
                StartCoroutine(nameof(SpawnLoop));
                return;
            }
            CalculateEnemiesAmount();
            ProcessWaveSpawns();
        }
        /*if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies)
        {
            randEnemy = Random.Range(0, spawnEnemy.Length);
            randPoint = Random.Range(0, spawnPoint.Length);
            GameObject enemy = Instantiate(spawnEnemy[randEnemy], spawnPoint[randPoint].transform.position, Quaternion.identity);
            enemy.transform.parent = enemyContainer;
            spawnerInterval = startSpawnerInterval;
            nowTheEnemies++;
        }
        else
        {
            spawnerInterval -= Time.deltaTime;
        }*/
    }

}
