using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int Damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        
        EnemyZombie enemyZombie = other.GetComponent<EnemyZombie>();// �������� ����������� �� ���� � �������� ����-�����

        if (enemyZombie != null)
        {
            enemyZombie.TakeDamage(Damage);
            //Debug.Log("������ ����");
        }

        Destroy(gameObject); //���������� ����
    }
}

