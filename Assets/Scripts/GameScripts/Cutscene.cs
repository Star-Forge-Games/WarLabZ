using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class Cutscene : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemySpawnSystem ess;
    [SerializeField] private Animator fader;
    [SerializeField] private GameObject pauseSystem;
    [SerializeField] private Timer timer;
    [SerializeField] private bool instaTimer = true;
    [SerializeField] private GameObject suppliesPanel;
    [SerializeField] private GameObject wagonSystem;
    [SerializeField] private GameObject wallHealthBar, waveText, pauseButton, moneyText, moneyImage;
    [SerializeField] private GameObject babahCube;
    [SerializeField] private ParticleSystem babah;

    private void Start()
    {
        KillsCount.kills = 0;
        KillsCount.bosses = 0;
        fader.gameObject.SetActive(true);
        if (instaTimer) timer.enabled = true;
        SkillsPanel.zombieSlow = false;
        SkillsPanel.lifesteal = false;
        SkillsPanel.zHealthReduction = false;
        SkillsPanel.bossHealthReduction = false;
    }

    public void Babah()
    {
        babah.Play();
        babah.GetComponent<AudioSource>().Play();
    }

    public void BabahCube()
    {
        babahCube.SetActive(true);
    }

    public void StartGame()
    {
        wallHealthBar.SetActive(true);
        waveText.SetActive(true);
        moneyText.SetActive(true);
        moneyImage.SetActive(true);
        pauseButton.SetActive(true);
        Destroy(GetComponent<Animator>());
        GetComponent<PositionConstraint>().constraintActive = true;
        GetComponent<AudioSource>().Play();
        player.enabled = true;
        ess.enabled = true;
        pauseSystem.SetActive(true);
        if (!instaTimer) timer.enabled = true;
        suppliesPanel.SetActive(true);
        Weapon.instance.SelfUnpause();
        wagonSystem.SetActive(true);
        Destroy(this);
    }
}
