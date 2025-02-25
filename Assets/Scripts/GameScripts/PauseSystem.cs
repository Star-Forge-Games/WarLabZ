using UnityEngine;

public class PauseSystem : MonoBehaviour
{

    private static PauseSystem instance;

    [SerializeField] PlayerController player;
    [SerializeField] Transform bullets, zombies;
    [SerializeField] GameObject pausePanel, winPanel;
    [SerializeField] Timer timer;

    private bool win = false;

    private bool paused = false;

    private void Awake()
    {
        instance = this;
    }

    public static void Win()
    {
        instance.win = true;
        instance.timer.Pause();
        instance.paused = true;
        instance.player.SaveMoney();
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

    public void OnPause()
    {
        if (win) return;
        if (instance.paused) Unpause();
        else Pause();
    }

    public static void Pause()
    {
        instance.timer.Pause();
        instance.paused = true;
        instance.player.Pause();
        foreach (Transform enemy in instance.zombies)
        {
            enemy.GetComponent<EnemyZombie>().Stop();
        }
        foreach (Transform bullet in instance.bullets)
        {
            bullet.GetComponent<Bullet>().Stop();
        }
        instance.pausePanel.SetActive(true);
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
    }

}
