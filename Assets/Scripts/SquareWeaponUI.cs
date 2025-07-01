using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SquareWeaponUI : MonoBehaviour
{
    public static Action<int, WeaponSettings, bool> WeaponTouched;
    private bool selected;
    [SerializeField] private int id;
    public WeaponSettings weaponSettings;

    private void Awake()
    {
        WeaponTouched += ProcessTouch;
    }

    public void ProcessButtonPress()
    {
        if (selected && WeaponUI.GetCurrentId() == id) return;
        if (selected && WeaponUI.GetCurrentId() != id)
        {
            WeaponTouched?.Invoke(id, weaponSettings, false);
            return;
        }
        if (YG2.saves.weaponLevels[id] != -1)
        {
            WeaponTouched?.Invoke(id, weaponSettings, true);
            return;
        }
        WeaponTouched?.Invoke(id, weaponSettings, false);
    }

    private void ProcessTouch(int id, WeaponSettings ws, bool selected)
    {
        if (!selected) return;
        if (this.id == id) Select();
        else Deselect();
    }

    public void Select()
    {
        selected = true;
        GetComponent<Image>().sprite = GunShopUIScript.sSprite;
    }

    public void Deselect()
    {
        selected = false;
        GetComponent<Image>().sprite = GunShopUIScript.nsSprite;
    }

    private void OnDestroy()
    {
        WeaponTouched -= ProcessTouch;
    }

}
