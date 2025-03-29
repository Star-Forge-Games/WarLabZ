
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitPrefab;


    private int damage;
    private float bulletLifeTime;
    private float speed;
    float timeLived = 0;
    private bool paused = true;
    private bool crit;
    private bool through;

    private void Start()
    {
        StartCoroutine(nameof(DestroyBullet));
    }

    private void Update()
    {
        if (!paused) timeLived += Time.deltaTime;
        if (transform.position.z > 105) Destroy(gameObject);
    }

    public void Setup(float speed, int damage, float bulletLifeTime, float critChance, float critMultiplier, bool instakill, bool through)
    {
        this.speed = speed;
        this.damage = damage;
        this.bulletLifeTime = bulletLifeTime;
        this.through = through;
        if (instakill)
        {
            if (Random.Range(0, 100f) > 5)
            {
                damage = 99999;
            }
        }
        else
        {
            float chance = Random.Range(0.0000001f, 100f);
            if (chance <= critChance)
            {
                this.damage = (int)(this.damage * critMultiplier);
                crit = true;
                // some other crit effect(visual maybe)
            }
        }
        SelfUnpause();
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(Mathf.Clamp(bulletLifeTime - timeLived, 0.01f, bulletLifeTime));
        Destroy(gameObject);
    }

    public void SelfPause()
    {
        paused = true;
        StopCoroutine(nameof(DestroyBullet));
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    public void SelfUnpause()
    {
        paused = false;
        StartCoroutine(nameof(DestroyBullet));
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyZombie>(out EnemyZombie z))
        {
            z.TakeDamage(damage);
            GameObject hitPrefabPoint = Instantiate(hitPrefab, transform.position - transform.forward * 1.2f, Quaternion.identity);
            if (!through)
            {
                Destroy(gameObject);
            }
        }
    }
}

