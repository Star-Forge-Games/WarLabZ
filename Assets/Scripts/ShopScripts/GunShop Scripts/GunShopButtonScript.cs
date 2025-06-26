using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GunShopUIScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI cash;
    private Action action;


    private void Start()
    {
        action = () =>
        {
            cash.text = $"{YG2.saves.cash} $";
        };
        WeaponUI.OnUpgrade += action;
    }

    private void OnDestroy()
    {
        WeaponUI.OnUpgrade -= action;
    }

}
