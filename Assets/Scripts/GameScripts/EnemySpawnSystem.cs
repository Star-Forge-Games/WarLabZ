using System;
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
    private bool paused = false;
    public int wave = 0;
    public int spawnedEnemies = 0, waveEnemies = 0;
    private Action<bool> action;
    private float endlessTimer;
    private bool endlessStarted;
    private PartProgress[] partsProgress;

    private struct PartProgress
    {
        public WavePart part;
        public int amountSpawned;
        public float timeSinceLastSpawn;
    }


    private void Awake()
    {
        EnemyZombie.OnZombieDie += ProcessZombieDeath;
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
    }
    void Start()
    {
        CalculateEnemiesAmount();
        ProcessWaveSpawns();
    }

    private void CalculateEnemiesAmount()
    {
        partsProgress = new PartProgress[waves[wave].parts.Length];
        for (int i = 0; i < partsProgress.Length; i++)
        {
            partsProgress[i] = new PartProgress { part = waves[wave].parts[i], amountSpawned = 0, timeSinceLastSpawn = 0 };
        }
        waveEnemies = 0;
        foreach (var part in waves[wave].parts)
        {
            waveEnemies += part.amount;
        }
    }

    private void ProcessWaveSpawns()
    {
        for (int i = 0; i < waves[wave].parts.Length; i++)
        {
            StartCoroutine(SpawnEnemies(i, wave == 0));
        }
    }

    private void ReProcessWaveSpawns()
    {
        for (int i = 0; i < partsProgress.Length; i++)
        {
            StartCoroutine(ReSpawnEnemies(i, wave == 0));
        }
    }

    private IEnumerator ReSpawnEnemies(int index, bool first)
    {
        WavePart part = partsProgress[index].part;
        if (partsProgress[index].amountSpawned == 0)
        {
            yield return new WaitForSeconds(part.delay + (first ? 0 : waves[wave - 1].nextWaveDelay) - partsProgress[index].timeSinceLastSpawn);
            for (int i = 0; i < part.amount; i++)
            {
                partsProgress[index].amountSpawned++;
                partsProgress[index].timeSinceLastSpawn = 0;
                SpawnEnemy(part.zombiePrefab, part.hpMultiplier != 0 ? part.hpMultiplier : 1);
                yield return new WaitForSeconds(part.interval);
            }
        }
        else
        {
            yield return new WaitForSeconds(part.interval - partsProgress[index].timeSinceLastSpawn);
            for (int i = 0; i < part.amount - partsProgress[index].amountSpawned; i++)
            {
                partsProgress[index].amountSpawned++;
                partsProgress[index].timeSinceLastSpawn = 0;
                SpawnEnemy(part.zombiePrefab, part.hpMultiplier != 0 ? part.hpMultiplier : 1);
                yield return new WaitForSeconds(part.interval);
            }
        }
    }

    private IEnumerator SpawnEnemies(int partIndex, bool first)
    {
        WavePart part = waves[wave].parts[partIndex];
        yield return new WaitForSeconds(part.delay + (first ? 0 : waves[wave - 1].nextWaveDelay));
        for (int i = 0; i < part.amount; i++)
        {
            partsProgress[partIndex].amountSpawned++;
            partsProgress[partIndex].timeSinceLastSpawn = 0;
            SpawnEnemy(part.zombiePrefab, part.hpMultiplier != 0 ? part.hpMultiplier : 1);
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
        EnemyZombie.OnZombieDie -= ProcessZombieDeath;
        PauseSystem.OnPauseStateChanged -= action;
    }

    private IEnumerator SpawnLoop()
    {
        if (endlessStarted)
        {
            yield return new WaitForSeconds(randomSpawnInterval - endlessTimer);
            while (true)
            {
                GameObject enemy = Instantiate(GetRandomEnemy(), GeneratePoint(), Quaternion.identity);
                enemy.transform.parent = enemyContainer;
                enemy.GetComponent<EnemyZombie>().MultiplyHp(1);
                endlessTimer = 0;
                yield return new WaitForSeconds(randomSpawnInterval);
            }
        }
        else
        {
            endlessStarted = true;
            yield return new WaitForSeconds(waves[wave - 1].nextWaveDelay);
            while (true)
            {
                GameObject enemy = Instantiate(GetRandomEnemy(), GeneratePoint(), Quaternion.identity);
                enemy.transform.parent = enemyContainer;
                enemy.GetComponent<EnemyZombie>().MultiplyHp(1);
                endlessTimer = 0;
                yield return new WaitForSeconds(randomSpawnInterval);
            }
        }
    }


    private Vector3 GeneratePoint()
    {
        return new Vector3(UnityEngine.Random.Range(-spawnpointWidth, spawnpointWidth), 1, spawnZ);
    }

    private GameObject GetRandomEnemy()
    {
        return enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
    }

    void Update()
    {
        if (paused) return;
        if (endlessStarted)
        {
            endlessTimer += Time.deltaTime;
            return;
        }
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
        for (int i = 0; i < partsProgress.Length; i++)
        {
            partsProgress[i].timeSinceLastSpawn += Time.deltaTime;
        }
    }

    public void ProcessZombieDeath(EnemyZombie z, float chance)
    {
        if (lastZombieSpawned && !endless && enemyContainer.childCount == 1)
        {
            PauseSystem.instance.Win();
        }
    }

    private void SelfPause()
    {
        paused = true;
        if (endlessStarted)
        {
            StopCoroutine(nameof(SpawnLoop));
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private void SelfUnpause()
    {
        if (endlessStarted)
        {
            StartCoroutine(nameof(SpawnLoop));
        }
        else
        {
            if (!lastZombieSpawned)
                ReProcessWaveSpawns();
        }
        paused = false;
    }


}