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
        if (Wall.instance != null) Wall.instance.TakeDamage(z, damage);
        Destroy(gameObject);
        Vector3 pos = this.transform.position;
        pos.z = 2.66f;
        pos.y = 1.04f;
        Instantiate(boomprefab, pos, Quaternion.identity);
    }

}
