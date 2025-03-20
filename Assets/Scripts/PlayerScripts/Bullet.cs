using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private int damage;
    private float bulletLifeTime;
    private float speed;
    float timeLived = 0;
    private bool paused = true;
    private bool crit;

    private void Start()
    {
        StartCoroutine(nameof(DestroyBullet));
    }

    private void Update()
    {
        if (!paused) timeLived += Time.deltaTime;
    }

    public void Setup(float speed, int damage, float bulletLifeTime, float critChance, float critMultiplier)
    {
        this.speed = speed;
        this.damage = damage;
        this.bulletLifeTime = bulletLifeTime;
        float chance = Random.Range(0.0000001f, 100f);
        if (chance <= critChance)
        {
            this.damage = (int) (this.damage * critMultiplier);
            crit = true;
            // some other crit effect(visual maybe)
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
            Destroy(gameObject);
        }
    }
}

