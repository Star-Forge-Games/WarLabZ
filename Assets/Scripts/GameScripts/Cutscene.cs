using UnityEngine;
using UnityEngine.Animations;

public class Cutscene : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemySpawnSystem ess;
    [SerializeField] private Animator fader;
    [SerializeField] private PauseSystem pauseSystem;
    [SerializeField] private Timer timer;
    [SerializeField] private bool instaTimer = true;

    private void Start()
    {
        fader.Play("Unfade");
    }

    public void StartGame()
    {
        Destroy(GetComponent<Animator>());
        GetComponent<PositionConstraint>().constraintActive = true;
        GetComponent<AudioSource>().Play();
        player.enabled = true;
        ess.enabled = true;
        pauseSystem.enabled = true;
        if (!instaTimer) timer.enabled = true;
        Destroy(this);
    }
}
