using System.Collections;
using UnityEngine;

public class OneShotRocket : MonoBehaviour
{

    [SerializeField] GameObject rocketPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireForce;
    [SerializeField] float rocketLifetime;


    public void Fire()
    {
        GameObject rocket = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();

        rocketRb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);

        Destroy(rocket, 1f);
    }






}
