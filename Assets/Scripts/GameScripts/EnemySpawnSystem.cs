using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnEnemy;
    [SerializeField] private Transform[] spawnPoint;
    
    public float startSpawnerInterval;
    private float spawnerInterval;

    public int numberOfEnemies;
    public int nowTheEnemies;

    public int randEnemy;
    private int randPoint;

   
    void Start()
    {
        spawnerInterval = startSpawnerInterval;
    }

    void Update()
    {
        if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies)
        {
            randEnemy = Random.Range(0, spawnEnemy.Length);
            randPoint = Random.Range(0, spawnPoint.Length);

            Instantiate(spawnEnemy[randEnemy], spawnPoint[randPoint].transform.position,Quaternion.identity);

            spawnerInterval = startSpawnerInterval;
            nowTheEnemies++;
        }
        else
        {
            spawnerInterval -= Time.deltaTime;
        }
    }
}
