using TMPro;
using UnityEngine;
using YG;

public class GunShopUIScript : MonoBehaviour
{

    [SerializeField] WeaponUI weaponUI;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] Sprite selectedSprite, notSelectedSprite;
    [SerializeField] Transform weaponGrid, handWeapons;
    public static Sprite sSprite, nsSprite;

    private void Awake()
    {
        SquareWeaponUI.WeaponTouched += ProcessTouch;
    }

    private void OnEnable()
    {
        sSprite = selectedSprite;
        nsSprite = notSelectedSprite;
        int weapon = YG2.saves.selectedWeapon;
        Refresh(weapon);
    }

    public void Refresh(int id)
    {
        Refresh();
        for (int i = 0; i < weaponGrid.childCount; i++)
        {
            SquareWeaponUI w = weaponGrid.GetChild(i).GetComponent<SquareWeaponUI>();
            if (i != id)
            {
                w.Deselect();
                handWeapons.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                w.Select();
                handWeapons.transform.GetChild(i).gameObject.SetActive(true);
                weaponUI.Setup(id, w.weaponSettings);
            }
        }
    }

    public void Refresh()
    {
        moneyText.text = $"{YG2.saves.cash} $";
    }

    private void ProcessTouch(int id, WeaponSettings ws, bool selected)
    {
        weaponUI.Setup(id, ws);
        if (selected)
        {
            YG2.saves.selectedWeapon = id;
            YG2.SaveProgress();
            Refresh(id);
        }
    }

    private void OnDestroy()
    {
        SquareWeaponUI.WeaponTouched -= ProcessTouch;
    }
}
