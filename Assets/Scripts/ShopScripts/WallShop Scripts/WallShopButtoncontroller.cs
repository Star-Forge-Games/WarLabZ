using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class WallShopButtonScript : MonoBehaviour
{

    [SerializeField] private GameObject[] walls;
    [SerializeField] private GameObject nextButton, previousButton;

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

    private void UpdateButtons()
    {
        nextButton.SetActive(lvl != walls.Length - 1);

        previousButton.SetActive(lvl != 0);

        wallsText.text = $"Уровень {lvl+1}\n{descriptions[lvl]}";
    }

    public void BackToShopScene()
    {
        SceneManager.LoadScene(3);
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
