using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GunShopUIScript : MonoBehaviour
{

    [SerializeField] private GameObject[] panels;
    [SerializeField] private TextMeshProUGUI cash;
    private int selectedPanel = 0;
    private Action action;


    private void Start()
    {
        YG2.saves.cash = 1000;
        YG2.saves.weaponLevels = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        YG2.SaveProgress();
        action = () =>
        {
            cash.text = $"{YG2.saves.cash} $";
        };
        WeaponUI.OnUpgrade += action;
        cash.text = $"{YG2.saves.cash} $";
        panels[0].SetActive(true);
    }

    public void ActivatePanel(int index)
    {
        panels[selectedPanel].SetActive(false);
        selectedPanel = index;
        for (int i = 0; i < panels[selectedPanel].transform.childCount; i++)
        {
            panels[selectedPanel].transform.GetChild(i).GetComponent<WeaponUI>().Refresh();
        }
        panels[selectedPanel].SetActive(true);
    }

    private void OnDestroy()
    {
        WeaponUI.OnUpgrade -= action;
    }

}
