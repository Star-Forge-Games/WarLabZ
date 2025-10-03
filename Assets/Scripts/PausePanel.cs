using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class PausePanel : MonoBehaviour
{

    [SerializeField] private Animator fader;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI skillsAmount;
    [SerializeField] private GameObject settingsPanel;

    public void AddSkill(Sprite skill, int num)
    {
        container.GetChild(num).GetComponent<Image>().sprite = skill;
        skillsAmount.text = $"{num + 1}/10";
    }

    public void RestartLevel()
    {
        SwitchScene(-1);
    }

    public void BackToHub()
    {
        if (YG2.saves.playedBefore != 1)
        {
            YG2.saves.playedBefore = 1;
            YG2.SaveProgress();
        }
        SwitchScene(0);
    }

    public void SwitchScene(int i)
    {
        fader.gameObject.SetActive(true);
        fader.Play("Fade");
        StartCoroutine(nameof(SwitchSceneCoroutine), i);
    }

    private IEnumerator SwitchSceneCoroutine(int i)
    {
        yield return new WaitForSeconds(1.2f);
        if (i != -1) SceneManager.LoadScene(i);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        settingsPanel.SetActive(false);
    }
}//
