using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Wave;
using static LocalizationHelperModule;

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
    [SerializeField] private TextMeshProUGUI waveText;
    private GameObject[] endlessWaveZombies;
    private int endlessZombieIndex = 0;
    private int endlessWave = 0;
    private bool lastZombieSpawned = false;
    private bool paused = false;
    [SerializeField] private int wave = 0;
    [SerializeField] private int spawnedEnemies = 0, waveEnemies = 0;
    private Action<bool> action;
    private float endlessTimer;
    private bool endlessStarted;
    private PartProgress[] partsProgress;

    public int GetTotalWave()
    {
        return wave + endlessWave;
    }

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
        int diff = endlessWave + endlessFirstWaveDifficulty;
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
        waveEnemies = endlessWaveZombies.Length;
    }

    private void ProcessWaveSpawnsEndless()
    {
        StartCoroutine(nameof(SpawnEnemiesEndless));
    }

    private IEnumerator SpawnEnemiesEndless()
    {
        yield return new WaitForSeconds(randomSpawnInterval - endlessTimer);
        for (int i = endlessZombieIndex; i < endlessWaveZombies.Length; i++)
        {
            GameObject enemy = Instantiate(endlessWaveZombies[i], GeneratePoint(), Quaternion.Euler(0, 180, 0));
            enemy.transform.parent = enemyContainer;
            enemy.GetComponent<EnemyZombie>().MultiplyHp(1);
            endlessZombieIndex++;
            endlessTimer = 0;
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    private void ProcessWaveSpawns()
    {
        for (int i = 0; i < waves[wave].parts.Length; i++)
        {
            StartCoroutine(nameof(SpawnEnemies), (i, wave == 0));
        }
    }

    private void ReProcessWaveSpawns()
    {
        for (int i = 0; i < partsProgress.Length; i++)
        {
            StartCoroutine(nameof(ReSpawnEnemies), (i, wave == 0));
        }
    }

    private IEnumerator ReSpawnEnemies((int index, bool first) data)
    {
        WavePart part = partsProgress[data.index].part;
        if (partsProgress[data.index].amountSpawned == 0)
        {
            yield return new WaitForSeconds(part.delay + (data.first ? 0 : waves[wave - 1].nextWaveDelay) - partsProgress[data.index].timeSinceLastSpawn);
            for (int i = 0; i < part.amount; i++)
            {
                partsProgress[data.index].amountSpawned++;
                partsProgress[data.index].timeSinceLastSpawn = 0;
                SpawnEnemy(part.zombiePrefab, part.hpMultiplier != 0 ? part.hpMultiplier : 1);
                yield return new WaitForSeconds(part.interval);
            }
        }
        else
        {
            yield return new WaitForSeconds(part.interval - partsProgress[data.index].timeSinceLastSpawn);
            for (int i = 0; i < part.amount - partsProgress[data.index].amountSpawned; i++)
            {
                partsProgress[data.index].amountSpawned++;
                partsProgress[data.index].timeSinceLastSpawn = 0;
                SpawnEnemy(part.zombiePrefab, part.hpMultiplier != 0 ? part.hpMultiplier : 1);
                yield return new WaitForSeconds(part.interval);
            }
        }
    }

    private IEnumerator SpawnEnemies((int partIndex, bool first) data)
    {
        WavePart part = waves[wave].parts[data.partIndex];
        yield return new WaitForSeconds(part.delay + (data.first ? 0 : waves[wave - 1].nextWaveDelay));
        for (int i = 0; i < part.amount; i++)
        {
            partsProgress[data.partIndex].amountSpawned++;
            partsProgress[data.partIndex].timeSinceLastSpawn = 0;
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
        waveText.text = $"{Loc("wave")}: {GetTotalWave() + 1}";
        if (endlessStarted)
        {
            endlessTimer += Time.deltaTime;
            if (endlessZombieIndex == waveEnemies)
            {
                if (enemyContainer.childCount == 0)
                {
                    spawnedEnemies = 0;
                    endlessZombieIndex = 0;
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
                if (wave >= waves.Length)
                {
                    CalculateEnemiesAmountEndless();
                    ProcessWaveSpawnsEndless();
                }
                else
                {
                    CalculateEnemiesAmount();
                    ProcessWaveSpawns();
                }
                if (wave % wavesPerSkill == 0)
                {
                    PauseSystem.instance.SkillSelect();
                }
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