using UnityEngine;

public class Wall : MonoBehaviour
{
    private int health;
    [SerializeField] public int maxHealth;

    private void Start()
    {
        EnemyZombie.OnZombieHitWall += TakeDamage;
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemyZombie.OnZombieHitWall -= TakeDamage;
            Destroy(gameObject);
            // death
        }
    }
}
