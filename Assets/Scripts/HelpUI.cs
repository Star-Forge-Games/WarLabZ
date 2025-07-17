using System;
using TMPro;
using UnityEngine;
using static LocalizationHelperModule;

public class HelpUI : MonoBehaviour
{
    private int page;

    [SerializeField] private string[] texts;
    [SerializeField] GameObject prevButton, nextButton;

    [SerializeField] TMP_Text text;
    private void OnEnable()
    {
        page = 0;
        ShowPage();
    }
    public void Next()
    {
        page++;
        ShowPage();
    }

    private void ShowPage()
    {
        prevButton.SetActive(page > 0);
        nextButton.SetActive(page < texts.Length-1);

        text.text = Loc(texts[page]);
    }

    public void Previous()
    {
        page--;
        ShowPage();
    }
}
