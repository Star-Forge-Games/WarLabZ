using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int Damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        
        EnemyZombie enemyZombie = other.GetComponent<EnemyZombie>();// Проверка столкнулась ли пуля с объектом Враг-Зомби

        if (enemyZombie != null)
        {
            enemyZombie.TakeDamage(Damage);
            //Debug.Log("Нанесён урон");
        }

        Destroy(gameObject); //Уничтожаем пулю
    }
}

