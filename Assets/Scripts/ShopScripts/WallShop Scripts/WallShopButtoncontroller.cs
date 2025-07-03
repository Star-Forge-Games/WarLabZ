using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static LocalizationHelperModule;

public class WallShopButtonScript : MonoBehaviour
{

    [SerializeField] private GameObject[] walls;
    [SerializeField] private GameObject nextButton, previousButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private WallSettings wallSettings;
    private int lvl;
    [SerializeField] TextMeshProUGUI wallsText;
    [SerializeField] Transform wallsTransform;
    [SerializeField] float speedRotation;
    [SerializeField] TextMeshProUGUI moneyText;



    private void Start()
    {
        lvl = YG2.saves.wallLevel;
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(i == lvl);
        }
        UpdateButtons();
    }
    private void Update()
    {
        wallsTransform.Rotate(0, Time.deltaTime * speedRotation, 0);
    }

    public void Upgrade()
    {
        YG2.saves.wallLevel = lvl;
        YG2.saves.cash -= wallSettings.wallLevels[lvl].cost;
        YG2.SaveProgress();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        moneyText.text = $"{YG2.saves.cash} $";
        nextButton.SetActive(lvl != walls.Length - 1);
        previousButton.SetActive(lvl != 0);
        wallsText.text = $"{Loc("level")} {lvl+1}\n{Loc($"wd{lvl+1}")}";
        if (YG2.saves.wallLevel >= lvl)
        {
            upgradeButton.interactable = false;
            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = Loc("bought");
        } else
        {
            if (YG2.saves.cash < wallSettings.wallLevels[lvl].cost)
            {
                upgradeButton.interactable = false;
                upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = Loc("nocash");
            }
            upgradeButton.interactable = true;
            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{Loc("buy")}\n{wallSettings.wallLevels[lvl].cost} $";
        }
    }

    public void Next()
    {
        walls[lvl].gameObject.SetActive(false);
        lvl++;
        walls[lvl].gameObject.SetActive(true);
        UpdateButtons();
    }

    public void Previous()
    {
        walls[lvl].gameObject.SetActive(false);
        lvl--;
        walls[lvl].gameObject.SetActive(true);
        UpdateButtons();
    }

    private void OnDisable()
    {
        foreach (GameObject wall in walls)
        {
            gameObject.SetActive(false);
        }
    }
}
