using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] float explosionRadius;
    [SerializeField] int damage;
    [SerializeField] GameObject explosion;
    void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -fallSpeed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        EnemyZombie[] objects = (from col in Physics.OverlapSphere(transform.position, explosionRadius) where col.gameObject.TryGetComponent<EnemyZombie>(out _) select col.GetComponent<EnemyZombie>()).ToArray();
        Instantiate(explosion, gameObject.transform.transform.position, Quaternion.identity);
        foreach (EnemyZombie obj in objects)
        {
            obj.TakeDamage(damage);
        }
    }
}
