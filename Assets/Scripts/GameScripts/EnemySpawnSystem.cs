using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Wave;

public class EnemySpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float randomSpawnInterval;
    [SerializeField] private float spawnZ;
    [SerializeField] private float spawnpointWidth;
    [SerializeField] private Wave[] waves;
    [SerializeField] private bool endless;
    [SerializeField] private int wavesPerSkill = 3;
    [SerializeField] private int endlessFirstWaveDifficulty = 45;
    private GameObject[] endlessWaveZombies;
    private int endlessZombieIndex = 0;
    private int endlessWave = 0;
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

    private void CalculateEnemiesAmountEndless()
    {
        waveEnemies = 0;
        int diff = wave + endlessFirstWaveDifficulty;
        int totalDiff = 0;
        endlessZombieIndex = 0;
        List<GameObject> totalZombies = new();
        while (totalDiff < diff)
        {
            var availableZombieList = (from z in enemyPrefabs where z.GetComponent<EnemyZombie>().GetDifficulty() <= (diff - totalDiff) select (z, z.GetComponent<EnemyZombie>().GetDifficulty())).ToArray();
            (GameObject prefab, int difficulty) zombieData = availableZombieList[UnityEngine.Random.Range(0, availableZombieList.Length)];
            totalDiff += zombieData.difficulty;
            totalZombies.Add(zombieData.prefab);
        }
        endlessWaveZombies = totalZombies.ToArray();
    }

    private void ProcessWaveSpawnsEndless()
    {
        StartCoroutine(SpawnEnemiesEndless());
    }

    private IEnumerator SpawnEnemiesEndless()
    {
        yield return new WaitForSeconds(randomSpawnInterval - endlessTimer);
        for (int i = endlessZombieIndex; i < endlessWaveZombies.Length; i++)
        {
            GameObject enemy = Instantiate(endlessWaveZombies[endlessZombieIndex], GeneratePoint(), Quaternion.Euler(0, 180, 0));
            enemy.transform.parent = enemyContainer;
            endlessZombieIndex++;
            endlessTimer = 0;
            yield return new WaitForSeconds(randomSpawnInterval);
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
        GameObject enemy = Instantiate(prefab, GeneratePoint(), Quaternion.Euler(0, 180, 0));
        enemy.transform.parent = enemyContainer;
        enemy.GetComponent<EnemyZombie>().MultiplyHp(mult);
    }

    void Update()
    {
        if (paused) return;
        if (endlessStarted)
        {
            endlessTimer += Time.deltaTime;
            if (endlessZombieIndex == waveEnemies)
            {
                if (enemyContainer.childCount == 0)
                {
                    spawnedEnemies = 0;
                    endlessWave++;
                    CalculateEnemiesAmountEndless();
                    ProcessWaveSpawnsEndless();
                }
            }
            return;
        }
        if (spawnedEnemies == waveEnemies)
        {
            if (enemyContainer.childCount == 0)
            {
                spawnedEnemies = 0;
                wave++;
                if (wave >= waves.Length)
                {
                    lastZombieSpawned = true;
                    endlessStarted = true;
                }
                if (wave % wavesPerSkill == 0)
                {
                    PauseSystem.instance.SkillSelect();
                }
                if (wave >= waves.Length)
                {
                    CalculateEnemiesAmountEndless();
                    ProcessWaveSpawnsEndless();
                    return;
                }
                CalculateEnemiesAmount();
                ProcessWaveSpawns();
            }
        }
        for (int i = 0; i < partsProgress.Length; i++)
        {
            partsProgress[i].timeSinceLastSpawn += Time.deltaTime;
        }
    }

    private void SelfPause()
    {
        paused = true;
        StopAllCoroutines();

    }

    private void SelfUnpause()
    {
        if (endlessStarted) ProcessWaveSpawnsEndless();
        else if (!lastZombieSpawned) ReProcessWaveSpawns();
        paused = false;
    }

    private Vector3 GeneratePoint()
    {
        return new Vector3(UnityEngine.Random.Range(-spawnpointWidth, spawnpointWidth), 1, spawnZ);
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
    }
}