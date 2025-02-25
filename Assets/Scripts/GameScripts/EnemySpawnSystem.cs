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
    [SerializeField] private Wave[] waves;
    [SerializeField] private bool endless;
    private bool lastZombieSpawned = false;

    public int wave = 0;
    public int spawnedEnemies = 0, waveEnemies = 0;

    private void Awake()
    {
        EnemyZombie.OnZombieHitPlayer += ProcessZombieHit;
        EnemyZombie.OnZombieDie += ProcessZombieDeath;
    }
    void Start()
    {
        CalculateEnemiesAmount();
        ProcessWaveSpawns();
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
            SpawnEnemy(part.zombiePrefab, part.hpMultiplier != 0 ? part.hpMultiplier:1);
            yield return new WaitForSeconds(part.interval);
        }
    }

    private void SpawnEnemy(GameObject prefab, float mult)
    {
        spawnedEnemies++;
        GameObject enemy = Instantiate(prefab, GeneratePoint(), Quaternion.identity);
        enemy.transform.parent = enemyContainer;
        enemy.GetComponent<EnemyZombie>().MultiplyHp(mult);
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitPlayer -= ProcessZombieHit;
        EnemyZombie.OnZombieDie -= ProcessZombieDeath;
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
                lastZombieSpawned = true;
                if (endless)
                {
                    StartCoroutine(nameof(SpawnLoop));
                }
                return;
            }
            CalculateEnemiesAmount();
            ProcessWaveSpawns();
        }

    }

    public void ProcessZombieDeath(EnemyZombie z, float chance)
    {
        if (lastZombieSpawned && !endless && enemyContainer.childCount == 0)
        {
            PauseSystem.Win();
        }
    }

}
