using System.Collections;
using UnityEngine;

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

    private int wave = 0;
    private int wavePart = 0;
    private int wavePartCounter = 0;

    /*public float startSpawnerInterval;
    private float spawnerInterval;
    public int numberOfEnemies;
    public int nowTheEnemies;
    public int randEnemy;
    private int randPoint;*/


    void Start()
    {
        //spawnerInterval = startSpawnerInterval;
        StartCoroutine(nameof(SpawnLoop));
        EnemyZombie.OnZombieHitPlayer += ProcessZombieHit;
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitPlayer -= ProcessZombieHit;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (wave >= waves.Length)
            {
                GameObject enemy = Instantiate(GetRandomEnemy(), GeneratePoint(), Quaternion.identity);
                enemy.transform.parent = enemyContainer;
                yield return new WaitForSeconds(randomSpawnInterval);
            }
            else
            {
                GameObject enemy = Instantiate(waves[wave].parts[wavePart].zombiePrefab, GeneratePoint(), Quaternion.identity);
                enemy.transform.parent = enemyContainer;
                wavePartCounter++;
                if (wavePartCounter == waves[wave].parts[wavePart].amount)
                {
                    wavePartCounter = 0;
                    yield return new WaitForSeconds(waves[wave].parts[wavePart].nextPartDelay);
                    wavePart++;
                    if (wavePart == waves[wave].parts.Length)
                    {
                        wavePart = 0;
                        wave++;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(waves[wave].parts[wavePart].interval);
                }
            }
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


    /*void Update()
    {
        if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies)
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
        }
    }*/

}
