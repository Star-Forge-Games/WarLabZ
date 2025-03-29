using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GunShopUIScript : MonoBehaviour
{

    [SerializeField] private GameObject[] panels;
    [SerializeField] private TextMeshProUGUI cash, dCash, level;
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
            dCash.text = $"{YG2.saves.donateCash} G";
            level.text = $"Lv. {YG2.saves.level + 1}";
        };
        WeaponUI.OnUpgrade += action;
        cash.text = $"{YG2.saves.cash} $";
        dCash.text = $"{YG2.saves.donateCash} G";
        level.text = $"Lv. {YG2.saves.level + 1}";
        panels[0].SetActive(true);
    }

    public void LoadShopScene()
    {
        SceneManager.LoadScene(3);
        // fader
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
