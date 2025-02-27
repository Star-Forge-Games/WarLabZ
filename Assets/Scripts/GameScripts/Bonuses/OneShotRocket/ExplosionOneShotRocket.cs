using UnityEngine;

public class ExplosionOneShotRocket : MonoBehaviour
{

    [SerializeField] float radius;
    [SerializeField] float force;

    [SerializeField] int damage;

    public bool Active;
    

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Explode()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, radius);
        
        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            Rigidbody rigidbodyZ  = overlappedColliders[i].attachedRigidbody;
            if (rigidbodyZ/*.TryGetComponent<EnemyZombie>(out EnemyZombie z)*/)
            {
                rigidbodyZ.AddExplosionForce(force,transform.position, radius);
                //z.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        Destroy(gameObject);
    }

}
