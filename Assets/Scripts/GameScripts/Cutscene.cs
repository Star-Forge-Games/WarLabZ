using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using YG;

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
    [SerializeField] private GameTutorial tutorial;

    private void Start()
    {
        fader.gameObject.SetActive(true);
        KillsCount.kills = 0;
        KillsCount.bosses = 0;
        SkillsPanel.zombieSlow = false;
        SkillsPanel.lifesteal = false;
        SkillsPanel.zHealthReduction = false;
        SkillsPanel.bossHealthReduction = false;
        if (YG2.saves.playedBefore == 0)
        {
            GetComponent<Animator>().speed = 0;
            tutorial.gameObject.SetActive(true);
            return;
        }
        if (instaTimer && timer != null) timer.enabled = true;
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

    public void StartAnim()
    {
        GetComponent<Animator>().speed = 1;
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
        suppliesPanel.SetActive(true);
        if (YG2.saves.playedBefore == 0)
        {
            tutorial.StartButtonTutorial();
            return;
        }
        GetComponent<AudioSource>().Play();
        player.enabled = true;
        ess.enabled = true;
        pauseSystem.SetActive(true);
        if (!instaTimer && timer != null) timer.enabled = true;
        Weapon.instance.SelfUnpause();
        wagonSystem.SetActive(true);
        Destroy(this);
    }

    public void StartTutorialGame()
    {
        GetComponent<AudioSource>().Play();
        player.enabled = true;
        ess.enabled = true;
        pauseSystem.SetActive(true);
        if (!instaTimer && timer != null) timer.enabled = true;
        Weapon.instance.SelfUnpause();
        wagonSystem.SetActive(true);
        Destroy(this);
    }
}
