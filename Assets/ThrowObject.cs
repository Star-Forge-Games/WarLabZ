using UnityEngine;

public class ThrowObject : MonoBehaviour
{

    private int damage;
    private EnemyZombie z;
    [SerializeField] GameObject boomprefab;

    public void Setup(EnemyZombie z, int damage)
    {
        this.z = z;
        this.damage = damage;
    }

    public void DoDamage()
    {
        Wall.instance.TakeDamage(z, damage);
        Destroy(gameObject);
        Vector3 pos = this.transform.position;
        pos.z = 1.5f;
        pos.y = 1.5f;
        Instantiate(boomprefab, pos, Quaternion.identity);
    }

}
