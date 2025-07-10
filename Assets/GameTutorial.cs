using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LocalizationHelperModule;

public class GameTutorial : MonoBehaviour
{

    [SerializeField] private Button contButton;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Cutscene cs;
    [SerializeField] private Image bg;
    [SerializeField] private Page[] pages;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private Transform finger;
    private int page = 0;
    private Color enabledColor, disabledColor;

    [Serializable]
    public struct Page
    {
        public string titleKey, textKey;
        public Vector2 fingerPosition;
        public float fingerRotationZ;
    }

    public void Start()
    {
        enabledColor = bg.color;
        disabledColor = bg.color;
        disabledColor.a = 0;
        StartCoroutine(nameof(TutorialText));
    }

    private IEnumerator TutorialText()
    {
        yield return new WaitForSeconds(2f);
        textBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        text.text = Loc("zombieactivity");
        contButton.interactable = true;
    }

    public void Next()
    {
        text.text = Loc("zombieboom");
        StartCoroutine(StartAnim());
        Destroy(contButton.gameObject);
    }

    private IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(3f);
        cs.StartAnim();
        yield return new WaitForSeconds(1f);
        textBox.SetActive(false);
        
    }

    public void StartButtonTutorial()
    {
        bg.gameObject.SetActive(true);
        pageText.text = $"<color=red>{Loc(pages[0].titleKey)}</color>\n{Loc(pages[0].textKey)}";
    }

    public void NextPage()
    {
        Page p = pages[page];
        Vector2 pos = p.fingerPosition;
        if (pageText.enabled)
        {
            if (pos != Vector2.zero)
            {
                pageText.enabled = false;
                bg.color = disabledColor;
                finger.gameObject.SetActive(true);
                float rot = p.fingerRotationZ;
                finger.transform.localEulerAngles = new Vector3(0, 0, rot);
                ((RectTransform)finger.transform).anchoredPosition = pos;
            }
            else
            {
                Debug.Log(page);
                page++;
                Debug.Log(page);
                if (page == pages.Length)
                {
                    gameObject.SetActive(false);
                    cs.StartTutorialGame();
                    return;
                }
                pageText.text = $"<color=red>{Loc(pages[page].titleKey)}</color>\n{Loc(pages[page].textKey)}";
            }
        }
        else
        {
            bg.color = enabledColor;
            finger.gameObject.SetActive(false);
            page++;
            pageText.text = $"<color=red>{Loc(pages[page].titleKey)}</color>\n{Loc(pages[page].textKey)}";
            pageText.enabled = true;
        }
    }
}
