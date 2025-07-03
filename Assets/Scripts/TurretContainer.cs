using UnityEngine;

public class TurretContainer : MonoBehaviour
{

    [SerializeField] Turret[] turrets;

    private void OnEnable()
    {
        foreach (Turret t in turrets)
        {
            t.enabled = true;
        }
    }

}
