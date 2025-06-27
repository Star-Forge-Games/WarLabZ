using UnityEngine;

public class ThrowObject : MonoBehaviour
{

    private int damage;
    private EnemyZombie z;

    public void Setup(EnemyZombie z, int damage)
    {
        this.z = z;
        this.damage = damage;
    }

    public void DoDamage()
    {
        Wall.instance.TakeDamage(z, damage);
        Destroy(gameObject);
    }

}
