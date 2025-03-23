using UnityEngine;

public class WeaponDataStorage : MonoBehaviour
{

    [SerializeField] WeaponSettings[] weaponsSettings;

    public static WeaponDataStorage instance;

    private void Awake()
    {
        instance = this;
    }

    public WeaponSettings GetWeaponSettings(int id)
    {
        return weaponsSettings[id];
    }

}
