using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage;

    private void Start()
    {
        StartCoroutine(nameof(DestroyBullet));
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void Stop()
    {
        StopCoroutine(nameof(DestroyBullet));
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
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

