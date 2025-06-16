using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{

    [SerializeField] Animator fader;

    public void RestartLevel()
    {
        SwitchScene(-1);
    }

    public void BackToHub()
    {
        SwitchScene(0);
    }

    public void SwitchScene(int i)
    {
        fader.Play("Fade");
        StartCoroutine(nameof(SwitchSceneCoroutine), i);
    }

    private IEnumerator SwitchSceneCoroutine(int i)
    {
        yield return new WaitForSeconds(1.2f);
        if (i != -1) SceneManager.LoadScene(i);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
