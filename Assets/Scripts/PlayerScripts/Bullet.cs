using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private int damage;


    private void Start()
    {
        StartCoroutine(nameof(DestroyBullet));
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*EnemyZombie enemyZombie = other.GetComponent<EnemyZombie>();
        if (enemyZombie != null)
        {
            enemyZombie.TakeDamage(Damage);
        }*/
        if (other.TryGetComponent<EnemyZombie>(out EnemyZombie z)) z.TakeDamage(damage);
        Destroy(gameObject);
    }

    public void Stop()
    {
        StopCoroutine(nameof(DestroyBullet));
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}

