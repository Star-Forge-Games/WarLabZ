using UnityEngine;
using UnityEngine.Animations;

public class LvL3Cutscene : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemySpawnSystem ess;
    [SerializeField] private Animator fader;
    [SerializeField] private GameObject pauseSystem;
    [SerializeField] private Timer timer;
    [SerializeField] private bool instaTimer = true;

    private void Start()
    {
        fader.Play("Unfade");
        if (instaTimer) timer.enabled = true;
    }

    public void StartGame()
    {
        Destroy(GetComponent<Animator>());
        GetComponent<PositionConstraint>().constraintActive = true;
        GetComponent<AudioSource>().Play();
        //Прописать 
        /*player.enabled = true;
        ess.enabled = true;
        pauseSystem.SetActive(true);
        if (!instaTimer) timer.enabled = true;
        Destroy(this);*/
    }
}
