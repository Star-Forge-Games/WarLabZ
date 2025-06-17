using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class WallShopButtonScript : MonoBehaviour
{

    [SerializeField] private GameObject[] walls;
    [SerializeField] private GameObject nextButton, previousButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private WallSettings wallSettings;

    private int lvl;
    [SerializeField] private string[] descriptions;
    [SerializeField] TextMeshProUGUI wallsText;
    [SerializeField] Transform wallsTransform;
    [SerializeField] float speedRotation;



    private void Start()
    {
        lvl = YG2.saves.wallLevel;
        walls[lvl].gameObject.SetActive(true);
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
    }

    private void UpdateButtons()
    {
        nextButton.SetActive(lvl != walls.Length - 1);
        previousButton.SetActive(lvl != 0);
        wallsText.text = $"Уровень {lvl+1}\n{descriptions[lvl]}";
        if (YG2.saves.wallLevel >= lvl)
        {
            upgradeButton.interactable = false;
            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Улучшено"; // localize
        } else
        {
            if (YG2.saves.cash < wallSettings.wallLevels[lvl].cost)
            {
                upgradeButton.interactable = false;
                upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Недостаточно денег"; // localize
            }
            upgradeButton.interactable = true;
            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Улучшить\n{wallSettings.wallLevels[lvl].cost} $"; // localize
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
}
