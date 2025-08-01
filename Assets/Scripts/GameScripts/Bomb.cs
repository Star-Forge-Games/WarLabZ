using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] float explosionRadius;
    [SerializeField] int damage;
    [SerializeField] MeshRenderer mr;
    private Action<bool> action;
    public int id;

    private void Awake()
    {
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
    }

    void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = -transform.up * fallSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyZombie[] objects = (from col in Physics.OverlapSphere(transform.position, explosionRadius) where col.gameObject.TryGetComponent<EnemyZombie>(out _) select col.GetComponent<EnemyZombie>()).ToArray();
        PauseSystem.OnPauseStateChanged -= action;
        ExplosionTestOptimization.instance.Activate(id);
        StartCoroutine(EnqueueDamage(objects));
        mr.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    private IEnumerator EnqueueDamage(EnemyZombie[] zombies)
    {
        foreach (EnemyZombie z in zombies)
        {
            yield return new WaitForEndOfFrame();
            if (z != null) z.TakeDamage(damage, false, false);
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
