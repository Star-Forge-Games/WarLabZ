using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SkillBomb : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] float explosionRadius;
    [SerializeField] int damage;
    [SerializeField] MeshRenderer mr;
    private Action<bool> action;
    private bool timed;

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
        GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -fallSpeed, 0);
    }

    public void Setup(bool timed)
    {
        this.timed = timed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyZombie[] objects = (from col in Physics.OverlapSphere(transform.position, explosionRadius) where col.gameObject.TryGetComponent<EnemyZombie>(out _) select col.GetComponent<EnemyZombie>()).ToArray();
        PauseSystem.OnPauseStateChanged -= action;
        BombSystem.instance.Explode(timed);
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
