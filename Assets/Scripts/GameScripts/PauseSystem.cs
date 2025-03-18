using System;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{

    public static PauseSystem instance;

    [SerializeField] PlayerController player;
    [SerializeField] Transform bullets, zombies;
    [SerializeField] GameObject pausePanel, winPanel;
    [SerializeField] Timer timer;

    public bool win = false;

    public bool paused = false;

    public static Action<bool> PauseChange;

    private void Awake()
    {
        instance = this;
    }

    public static void Win()
    {
        instance.win = true;
        PauseChange?.Invoke(true);
        instance.timer.Pause();
        instance.paused = true;
        MoneySystem.SaveMoney();
        instance.player.Pause();
        foreach (Transform enemy in instance.zombies)
        {
            enemy.GetComponent<EnemyZombie>().Stop();
        }
        foreach (Transform bullet in instance.bullets)
        {
            bullet.GetComponent<Bullet>().Stop();
        }
        instance.winPanel.SetActive(true);
    }

    public static void Pause()
    {
        instance.timer.Pause();
        instance.paused = true;
        instance.pausePanel.SetActive(true);
        PauseChange?.Invoke(true);
        instance.player.Pause();
        foreach (Transform enemy in instance.zombies)
        {
            enemy.GetComponent<EnemyZombie>().Stop();
        }
        foreach (Transform bullet in instance.bullets)
        {
            bullet.GetComponent<Bullet>().Stop();
        }
    }

    public static void Unpause()
    {
        instance.timer.Unpause();
        instance.paused = false;
        instance.player.Unpause();
        foreach (Transform enemy in instance.zombies)
        {
            enemy.GetComponent<EnemyZombie>().Unpause();
        }
        foreach (Transform bullet in instance.bullets)
        {
            bullet.GetComponent<Bullet>().Unpause();
        }
        instance.pausePanel.SetActive(false);
        PauseChange?.Invoke(false);
    }

}
