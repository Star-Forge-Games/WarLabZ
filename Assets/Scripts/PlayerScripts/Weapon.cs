using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletsRate;
    [SerializeField] float fireForce;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletLifeTime;

    [SerializeField] private Transform bulletContainer;

    [SerializeField] GameObject bulletPrefab;

   
    private float flatRateModifier = 0, expRateModifier = 1;
    private float flatDamageModifier = 0, expDamageModifier = 1;
    private float flatSpeedModifier = 0, expSpeedModifier = 1;


    private IEnumerator PeriodicFireSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(bulletsRate);
            Fire();
        }
    }

    private void Fire()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>(); //создаём снаряд из префаба
        bullet.transform.parent = bulletContainer;
        bullet.Launch(fireForce, firePoint.forward);
    }

    public void Pause()
    {
        StopCoroutine(nameof(PeriodicFireSpawn));
        foreach (Transform bullet in bulletContainer)
        {
            bullet.GetComponent<Bullet>().Stop();
        }
    }

    public void UnPause()
    {
        StartCoroutine(nameof(PeriodicFireSpawn));
        foreach (Transform bullet in bulletContainer)
        {
            bullet.GetComponent<Bullet>().Stop();
        }
    }

}
