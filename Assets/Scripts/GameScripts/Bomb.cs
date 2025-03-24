using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] float explosionRadius;
    [SerializeField] int damage;
    [SerializeField] GameObject explosion;
    private Action<bool> action;

    private void Awake()
    {
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
    }

    void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -fallSpeed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyZombie[] objects = (from col in Physics.OverlapSphere(transform.position, explosionRadius) where col.gameObject.TryGetComponent<EnemyZombie>(out _) select col.GetComponent<EnemyZombie>()).ToArray();
       PauseSystem.OnPauseStateChanged -= action;
        Quaternion q = Quaternion.Euler(-90, 0, 0);
        Instantiate(explosion, gameObject.transform.transform.position, q);
        StartCoroutine(EnqueueDamage(objects));
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    private IEnumerator EnqueueDamage(EnemyZombie[] zombies)
    {
        foreach (EnemyZombie z in zombies)
        {
            yield return new WaitForEndOfFrame();
            if (z != null) z.TakeDamage(damage);
        }
        Destroy(gameObject);
    }


    private void SelfPause()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    private void SelfUnpause()
    {
        GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -fallSpeed, 0);
    }

    
}
