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
    [SerializeField] private TextMeshProUGUI text, waveText;
    [SerializeField] private Cutscene cs;
    [SerializeField] private Image bg;
    [SerializeField] private Page[] pages;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private Transform finger;
    private int page = 0;

    [Serializable]
    public struct Page
    {
        public string titleKey, textKey;
        public Vector2 fingerPosition;
        public float fingerRotationZ;
    }

    public void Start()
    {
        waveText.text = Loc("wave0");
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
        ShowPage();
    }

    public void ShowPage()
    {
        Page p = pages[page];
        Vector2 pos = p.fingerPosition;
        pageText.text = $"<color=red>{Loc(pages[page].titleKey)}</color>\n{Loc(pages[page].textKey)}";
        if (pos != Vector2.zero)
        {
            finger.gameObject.SetActive(true);
            float rot = p.fingerRotationZ;
            finger.transform.localEulerAngles = new Vector3(0, 0, rot);
            ((RectTransform)finger.transform).anchoredPosition = pos;
        }
        else
        {
            finger.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        page++;
        if (page == pages.Length)
        {
            gameObject.SetActive(false);
            cs.StartTutorialGame();
            return;
        }
        ShowPage();
    }
}
