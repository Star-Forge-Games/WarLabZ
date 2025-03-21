using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using YG;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletsRate;
    [SerializeField] float fireForce;
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletLifeTime;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private int weaponId;
    [Header("Shotgun Settings")]
    [SerializeField] private bool shotgun;
    [ShowIf("shotgun")]
    [SerializeField] private int shots;
    [ShowIf("shotgun")]
    [SerializeField] private float arc;
    [Header("Multishot Settings")]
    [Range(0f, 2f)]
    [SerializeField] private int multishots = 1;
    [Range(0.01f, 0.33f)]
    [SerializeField] private float baseAttackTimePercentage = 0.1f;

    private float flatRateModifier = 0, expRateModifier = 1;
    private float flatDamageModifier = 0, expDamageModifier = 1;
    private float flatSpeedModifier = 0, expSpeedModifier = 1;
    private float critChance = 75, critDamageMultiplier = 2;

    private void Awake()
    {
        if (YG2.saves.selectedWeapon != weaponId)
        {
            gameObject.SetActive(false);
            return;
        }
        // attach upgrades from SAVES YG
    }


    private IEnumerator PeriodicFireSpawn()
    {
        while (true)
        {
            if (multishots > 0)
            {
                float rate = 1f / ((bulletsRate + flatRateModifier) * expRateModifier);
                yield return new WaitForSeconds(rate * (1-baseAttackTimePercentage * multishots));
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
        if (!shotgun)
        {
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
            bullet.transform.parent = bulletContainer;
            bullet.Setup((fireForce + flatSpeedModifier) * expSpeedModifier, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier);
        } else
        {
            for (int i = 0; i < shots; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                bullet.transform.Rotate(0, -(arc / 2) + (arc/(shots-1)) * i, 0);
                bullet.transform.parent = bulletContainer;
                bullet.Setup((fireForce + flatSpeedModifier) * expSpeedModifier, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier);
            }
        }
    }

    private void Start()
    {
        SelfUnpause();
    }

    public void SelfPause()
    {
        StopCoroutine(nameof(PeriodicFireSpawn));
    }

    public void SelfUnpause()
    {
        StartCoroutine(nameof(PeriodicFireSpawn));
        
    }

    public void IncreaseRateModifier(bool flat, float value)
    {
        if (flat) flatRateModifier += value;
        else expRateModifier += value;
    }

    public void IncreaseDamageModifier(bool flat, float value)
    {
        if (flat) flatDamageModifier += value;
        else expDamageModifier += value;
    }

    public void IncreaseSpeedModifier(bool flat, float value)
    {
        if (flat) flatSpeedModifier += value;
        else expSpeedModifier += value;
    }

    public void IncreaseCritChance(float value)
    {
        critChance += value;
    }

    public void IncreaseCritDamage(float value)
    {
        critChance += value;
    }

}
