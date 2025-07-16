using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LocalizationHelperModule;


public class SkillInfoUI : MonoBehaviour
{
    private int page;
    [SerializeField] GameObject prevButton, nextButton;
    [SerializeField] Image[] images;
    [SerializeField] TMP_Text[] texts;
    [SerializeField] SkillsPanel.Modifier[] skills;



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
    public void Previous()
    {
        page--;
        ShowPage();
    }

    private void ShowPage()
    {
        if (page == 0)
        {
            prevButton.SetActive(false);
            nextButton.SetActive(true);
        }

        else if (page == 6)
        {
            prevButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else
        {
            prevButton.SetActive(true);
            nextButton.SetActive(true);
        }

        for (int i = page * 4; i < page * 4 + 4; i++)
        {
            if (i >= skills.Length)
            {
                images[i % 4].gameObject.SetActive(false);
                texts[i % 4].gameObject.SetActive(false);
            }
            else
            {
                images[i % 4].gameObject.SetActive(true);
                texts[i % 4].gameObject.SetActive(true);

                images[i % 4].sprite = skills[i].sprite;
                texts[i % 4].text = Loc(skills[i].key);
            }

        }

    }

}
