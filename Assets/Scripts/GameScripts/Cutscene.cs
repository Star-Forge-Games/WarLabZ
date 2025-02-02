using UnityEngine;
using UnityEngine.Animations;

public class Cutscene : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemySpawnSystem ess;
    [SerializeField] Animator fader;

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
        Destroy(this);
    }
}
