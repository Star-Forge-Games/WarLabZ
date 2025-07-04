using UnityEngine;

public class TurretContainer : MonoBehaviour
{

    [SerializeField] Turret[] turrets;

    public void Setup(Transform bulletContainer, Transform enemyContainer)
    {
        foreach (Turret t in turrets)
        {
            t.enabled = true;
            t.Setup(bulletContainer, enemyContainer);
        }
    }
}
