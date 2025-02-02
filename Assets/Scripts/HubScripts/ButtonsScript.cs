using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{

    [SerializeField] private Animator fader;

    public void StartGame()
    {
        fader.Play("Fade");
        StartCoroutine(nameof(Game));
    }

    private IEnumerator Game()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(1);
    }

}
