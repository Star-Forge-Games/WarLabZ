using System.Collections;
using UnityEngine;

public class LavaAbility : MonoBehaviour
{
    [SerializeField] GameObject lava, rain;
    [SerializeField] int damageTicks;
    [SerializeField] int damage;
    [SerializeField] float damageDelay;
    [SerializeField] Transform enemyContainer;

    public void Fire()
    {
        rain.SetActive(true);
        StartCoroutine(DamageCoroutine());
        StartCoroutine(WetFloor());
    }

    public IEnumerator DamageCoroutine()
    {
        for (int i = 0; i < damageTicks; i++)
        {
            yield return new WaitForSeconds(damageDelay);
            foreach (Transform t in enemyContainer)
            {
                EnemyZombie z = t.GetComponent<EnemyZombie>();
                yield return new WaitForEndOfFrame();
                if (z != null)
                z.TakeDamage(damage);
            }
        }
        lava.SetActive(false);
        rain.SetActive(false);
    }

    public IEnumerator WetFloor()
    {
        yield return new WaitForSeconds(1);
        lava.SetActive(true);
    }
}
