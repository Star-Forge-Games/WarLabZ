using System;
using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform[] firePoints;
    [SerializeField] float bulletsRate;
    [SerializeField] float fireForce;
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletLifeTime;
    [SerializeField] private float range;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] GameObject bulletPrefab;
    [Header("Shotgun Settings")]
    [SerializeField] private bool shotgun;
    [SerializeField] private int shots;
    [SerializeField] private float arc;
    [Header("Multishot Settings")]
    [Range(0f, 2f)]
    [SerializeField] private int multishots = 1;
    [Range(0.01f, 0.33f)]
    [SerializeField] private float baseAttackTimePercentage = 0.1f;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float twinShotDistance = 0.1f;
    private float flatRateModifier = 0, expRateModifier = 1;
    private float flatDamageModifier = 0, expDamageModifier = 1;
    private float flatSpeedModifier = 0, expSpeedModifier = 1;
    private float critChance = 75, critDamageMultiplier = 2;
    private bool targeting = false;
    Vector3 targetPosition;
    private Action<bool> action;
    private Action<int> skillAction;
    private bool twinShot, instakill, through;

    private void Awake()
    {
        skillAction = id =>
        {
            switch (id)
            {
                case 20:
                    twinShot = true;
                    break;
                case 21:
                    critChance += 10;
                    break;
                case 22:
                    flatRateModifier += 0.5f;
                    break;
                case 23:
                    through = true;
                    break;
                case 24:
                    multishots = 1;
                    break;
                case 25:
                    flatDamageModifier += 1;
                    break;
                default:
                    critDamageMultiplier += 0.1f;
                    break;
            };
        };
        SkillsPanel.OnTurretSkillSelect += skillAction;
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
    }

    private void Update()
    {
        Transform target = null;
        float min = range;
        foreach (Transform t in enemyContainer)
        {
            if (t.position.z < range)
            {
                if (t.position.z < min)
                {
                    min = t.position.z;
                    target = t;
                }
            }
        }
        if (targeting)
        {
            if (target == null)
            {
                StopCoroutine(nameof(PeriodicFireSpawn));
                targeting = false;
            }
            else
            {
                targetPosition = target.position;
                RotateTowardsTarget();
            }
        }
        else
        {
            if (target != null)
            {
                targetPosition = target.position;
                RotateTowardsTarget();
                StartCoroutine(nameof(PeriodicFireSpawn));
                targeting = true;
            }
        }
    }

    private void RotateTowardsTarget()
    {
        Quaternion q = Quaternion.LookRotation(targetPosition - transform.position);
        q.x = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }

    public void SelfPause()
    {
        StopCoroutine(nameof(PeriodicFireSpawn));
    }

    public void SelfUnpause()
    {
        StartCoroutine(nameof(PeriodicFireSpawn));
    }

    private IEnumerator PeriodicFireSpawn()
    {
        while (true)
        {
            if (multishots > 0)
            {
                float rate = 1f / ((bulletsRate + flatRateModifier) * expRateModifier);
                yield return new WaitForSeconds(rate * (1 - baseAttackTimePercentage * multishots));
                Fire();
                for (int i = 0; i < multishots; i++)
                {
                    yield return new WaitForSeconds(rate * baseAttackTimePercentage);
                    Fire();
                }
            }
            else
            {
                yield return new WaitForSeconds(1f / ((bulletsRate + flatRateModifier) * expRateModifier));
                Fire();
            }
        }
    }

    private void Fire()
    {
        foreach (Transform firePoint in firePoints)
        {
            if (!shotgun)
            {
                if (twinShot)
                {
                    Bullet b1 = Instantiate(bulletPrefab, firePoint.position - firePoint.right * twinShotDistance, firePoint.rotation).GetComponent<Bullet>();
                    b1.GetComponent<LineTest>().Setup((bulletsRate + flatRateModifier) * expRateModifier);
                    b1.transform.parent = bulletContainer;
                    b1.Setup((fireForce + flatSpeedModifier) * expSpeedModifier, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier, instakill, through, false, 0);
                    Bullet b2 = Instantiate(bulletPrefab, firePoint.position + firePoint.right * twinShotDistance, firePoint.rotation).GetComponent<Bullet>();
                    b2.GetComponent<LineTest>().Setup((bulletsRate + flatRateModifier) * expRateModifier);
                    b2.transform.parent = bulletContainer;
                    b2.Setup((fireForce + flatSpeedModifier) * expSpeedModifier, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier, instakill, through, false, 0);
                }
                else
                {
                    Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                    bullet.GetComponent<LineTest>().Setup((bulletsRate + flatRateModifier) * expRateModifier);
                    bullet.transform.parent = bulletContainer;
                    bullet.Setup((fireForce + flatSpeedModifier) * expSpeedModifier, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier, instakill, through, false, 0);
                }
            }
            else
            {
                for (int i = 0; i < shots; i++)
                {
                    Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                    bullet.GetComponent<LineTest>().Setup((bulletsRate + flatRateModifier) * expRateModifier);
                    bullet.transform.Rotate(0, -(arc / 2) + (arc / (shots - 1)) * i, 0);
                    bullet.transform.parent = bulletContainer;
                    bullet.Setup((fireForce + flatSpeedModifier) * expSpeedModifier, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier, instakill, through, false, 0);
                }
            }
        }
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
        SkillsPanel.OnTurretSkillSelect -= skillAction;
    }
}
