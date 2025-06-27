using System;
using System.Collections;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{

    public static PauseSystem instance;

    [SerializeField] private GameObject pausePanel, winPanel, losePanel, skillsPanel;
    [SerializeField] private Transform bulletContainer, enemyContainer, wagonContainer, stoneContainer;
    private bool selectingSkill;
    private bool end = false;
    public static Action<bool> OnPauseStateChanged;

    private void Awake()
    {
        instance = this;
    }

    public void Lose()
    {
        MoneySystem.instance.SaveMoney();
        end = true;
        StartCoroutine(WaitForLose());
    }

    private IEnumerator WaitForLose()
    {
        yield return new WaitForSeconds(2);
        Pause(true);
        losePanel.SetActive(true);
    }

    public void Win()
    {
        MoneySystem.instance.SaveMoney();
        end = true;
        Pause(true);
        winPanel.SetActive(true);
    }

    public void SkillSelect()
    {
        if (skillsPanel.GetComponent<SkillsPanel>().ReachedMaxSkillLimit()) return;
        selectingSkill = true;
        Pause(false);
        skillsPanel.SetActive(true);
    }

    public void Pause(bool end)
    {
        if (this.end && !end) return;
        OnPauseStateChanged?.Invoke(true);
        foreach (Transform t in enemyContainer)
        {
            t.GetComponent<EnemyZombie>().SelfPause();
        }
        foreach (Transform t in bulletContainer)
        {
            t.GetComponent<Bullet>().SelfPause();
        }
        foreach (Transform t in wagonContainer)
        {
            t.GetComponent<Wagon>().SelfPause();
        }
        foreach (Transform t in stoneContainer)
        {
            t.GetComponent<Animator>().speed = 0;
        }
        if (selectingSkill) return;
        if (!end) pausePanel.SetActive(true);
    }

    public void Unpause(bool fromSkillSelection)
    {
        if (end) return;
        if (fromSkillSelection) selectingSkill = false;
        if (selectingSkill && !fromSkillSelection) return;
        OnPauseStateChanged?.Invoke(false);
        foreach (Transform t in enemyContainer)
        {
            t.GetComponent<EnemyZombie>().SelfUnpause();
        }
        foreach (Transform t in bulletContainer)
        {
            t.GetComponent<Bullet>().SelfUnpause();
        }
        foreach (Transform t in wagonContainer)
        {
            t.GetComponent<Wagon>().SelfUnpause();
        }
        foreach (Transform t in stoneContainer)
        {
            t.GetComponent<Animator>().speed = 1;
        }
        pausePanel.SetActive(false);
    }

}
