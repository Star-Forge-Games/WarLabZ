using System;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{

    public static PauseSystem instance;

    [SerializeField] private GameObject pausePanel, winPanel, losePanel, skillsPanel;
    [SerializeField] private Transform bulletContainer, enemyContainer;

    private bool end = false;

    public static Action<bool> OnPauseStateChanged;

    private void Awake()
    {
        instance = this;
    }

    public void Lose()
    {
        end = true;
        Pause(true);
        losePanel.SetActive(true);
    }

    public void Win()
    {
        end = true;
        Pause(true);
        winPanel.SetActive(true);
    }

    public void SkillSelect()
    {
        OnPauseStateChanged?.Invoke(true);
        foreach (Transform t in enemyContainer)
        {
            t.GetComponent<EnemyZombie>().SelfPause();
        }
        foreach (Transform t in bulletContainer)
        {
            t.GetComponent<Bullet>().SelfPause();
        }
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
       if (!end) pausePanel.SetActive(true);
    }

    public void Unpause()
    {
        if (end) return;
        OnPauseStateChanged?.Invoke(false);
        foreach (Transform t in enemyContainer)
        {
            t.GetComponent<EnemyZombie>().SelfUnpause();
        }
        foreach (Transform t in bulletContainer)
        {
            t.GetComponent<Bullet>().SelfUnpause();
        }
        pausePanel.SetActive(false);
    }

}
