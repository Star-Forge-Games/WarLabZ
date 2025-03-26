using System;
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

    [SerializeField] GameObject gunFireFront;
    [SerializeField] GameObject gunFireBack;

    private float flatRateModifier = 0, expRateModifier = 1;
    private float flatDamageModifier = 0, expDamageModifier = 1;
    [SerializeField] private float critChance = 75, critDamageMultiplier = 2;

    private Action<bool> action;

    private void Awake()
    {
        if (YG2.saves.selectedWeapon != weaponId)
        {
            gameObject.SetActive(false);
            return;
        }
        int level = YG2.saves.weaponLevels[weaponId];
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
        WeaponSettings settings = WeaponDataStorage.instance.GetWeaponSettings(weaponId);
        WeaponSettings.Level l = settings.levels[level];
        bulletDamage += l.damageBuff;
        critChance += l.critChanceBuff;
        critDamageMultiplier += l.critBuff;
        bulletsRate += l.aspdBuff;
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
        GameObject gunFireFrontFirePoint = Instantiate(gunFireFront, firePoint.position, firePoint.rotation);
        gunFireFrontFirePoint.transform.parent = firePoint;
        Destroy(gunFireFrontFirePoint,0.03f);        

        GameObject gunFireBackFirePoint = Instantiate(gunFireBack, firePoint.position, firePoint.rotation);
        gunFireBackFirePoint.transform.parent = firePoint;
        Destroy(gunFireBackFirePoint,0.03f);


        if (!shotgun)
        {
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
            bullet.transform.parent = bulletContainer;
            bullet.Setup(fireForce, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier);
        } else
        {
            for (int i = 0; i < shots; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                bullet.transform.Rotate(0, -(arc / 2) + (arc/(shots-1)) * i, 0);
                bullet.transform.parent = bulletContainer;
                bullet.Setup(fireForce, (int)((bulletDamage + flatDamageModifier) * expDamageModifier), bulletLifeTime, critChance, critDamageMultiplier);
            }
        }
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

    public void IncreaseCritChance(float value)
    {
        critChance += value;
    }

    public void IncreaseCritDamage(float value)
    {
        critChance += value;
    }

    private void OnDestroy()
    {
        if (action != null) PauseSystem.OnPauseStateChanged -= action;
    }

}
