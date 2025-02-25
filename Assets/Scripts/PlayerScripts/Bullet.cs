using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private int damage;
    private float bulletLifeTime;
    private float speed;
    float timeLived = 0;
    private bool paused = true;

    private void Start()
    {
        StartCoroutine(nameof(DestroyBullet));
    }

    private void Update()
    {
        if (!paused) timeLived += Time.deltaTime;
    }

    public void Setup(float speed, int damage, float bulletLifeTime)
    {
        this.speed = speed;
        this.damage = damage;
        this.bulletLifeTime = bulletLifeTime;
        Unpause();
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(Mathf.Clamp(bulletLifeTime - timeLived, 0.01f, bulletLifeTime));
        Destroy(gameObject);
    }

    public void Stop()
    {
        paused = true;
        StopCoroutine(nameof(DestroyBullet));
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    public void Unpause()
    {
        paused = false;
        StartCoroutine(nameof(DestroyBullet));
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }

    public void Launch(float fireForce, Vector3 direction)
    {
        StartCoroutine(nameof(DestroyBullet));
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * fireForce, ForceMode.Impulse);
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

