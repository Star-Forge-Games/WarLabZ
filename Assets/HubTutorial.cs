using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using static LocalizationHelperModule;

public class HubTutorial : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator finger;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Page[] pages;
    [SerializeField] private Image bg;
    private Color enabledColor, disabledColor;

    [Serializable]
    public struct Page
    {
        public string titleKey, textKey;
        public Vector2 fingerPosition;
        public float fingerRotationZ;
        public int buttonId;
    }
    private int page;

    private void OnEnable()
    {
        enabledColor = bg.color;
        disabledColor = bg.color;
        disabledColor.a = 0;
        page = 0;
        text.text = $"<color=red>{Loc(pages[0].titleKey)}</color>\n{Loc(pages[0].textKey)}";
        foreach (var b in buttons)
        {
            b.interactable = false;
            b.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        Page p = pages[page];
        int bid = p.buttonId;
        Vector2 pos = p.fingerPosition;
        if (text.enabled)
        {
            if (pos != Vector2.zero)
            {
                text.enabled = false;
                bg.color = disabledColor;
                finger.gameObject.SetActive(true);
                float rot = p.fingerRotationZ;
                finger.transform.localEulerAngles = new Vector3(0, 0, rot);
                ((RectTransform)finger.transform).anchoredPosition = pos;
                if (bid != -1) buttons[bid].gameObject.SetActive(true);
            }
            else
            {
                page++;
                if (page == pages.Length)
                {
                    YG2.saves.playedBefore = 0;
                    YG2.SaveProgress();
                    foreach (var b in buttons)
                    {
                        b.gameObject.SetActive(true);
                        b.interactable = true;
                    }
                    gameObject.SetActive(false);
                    SceneManager.LoadScene("GameWorld");
                    return;
                }
                text.text = $"<color=red>{Loc(pages[page].titleKey)}</color>\n{Loc(pages[page].textKey)}";
            }
        } else
        {
            bg.color = enabledColor;
            finger.gameObject.SetActive(false);
            page++;
            text.text = $"<color=red>{Loc(pages[page].titleKey)}</color>\n{Loc(pages[page].textKey)}";
            text.enabled = true;
        }
    }
}
