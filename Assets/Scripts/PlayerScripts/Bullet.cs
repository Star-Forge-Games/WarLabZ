
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitPrefab;
    [SerializeField] float bombChance = 5, instaKillChance = 99f;


    private int damage;
    private float bulletLifeTime;
    private float speed;
    float timeLived = 0;
    private bool paused = true;
    private bool crit;
    private bool through;
    private bool bomb;
    private float stunChance;
    private bool instaKill;

    private void Start()
    {
        StartCoroutine(nameof(DestroyBullet));
    }

    private void Update()
    {
        if (!paused) timeLived += Time.deltaTime;
    }

    public void Setup(float speed, int damage, float bulletLifeTime, float critChance, float critMultiplier, bool instakill, bool through, bool bomb, float stunChance)
    {
        if (bomb)
        {
            if (Random.Range(0, 100f) < bombChance)
            {
                this.bomb = true;
            }
        }
        this.speed = speed;
        this.damage = damage;
        this.bulletLifeTime = bulletLifeTime;
        this.through = through;
        this.stunChance = stunChance;
        if (instakill)
        {
            if (Random.Range(0, 100f) < instaKillChance)
            {
                this.instaKill = true;
            }
        }
        else
        {
            float chance = Random.Range(0.0000001f, 100f);
            if (chance <= critChance)
            {
                int dmg = (int)(this.damage * critMultiplier);
                if (this.damage == dmg) this.damage++;
                else this.damage = dmg;
                crit = true;
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
            z.TakeDamage(damage, crit, instaKill);
            Instantiate(hitPrefab, transform.position - transform.forward * 1.2f, Quaternion.identity);
            if (Random.Range(0, 100f) < stunChance)
            {
                if (!z.IsDead()) z.Stun();
            }
            if (bomb)
            {
                BombSystem.instance.ThrowAt(transform.position.x, transform.position.z);
            }
            if (!through)
            {
                Destroy(gameObject);
            }
        }
    }
}

