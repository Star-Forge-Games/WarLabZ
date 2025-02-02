using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{

    [SerializeField] Animator fader;

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void BackToHub()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    /*public void SwitchScene(int i)
    {
        fader.Play("Fade");
        StartCoroutine(nameof(SwitchSceneCoroutine), i);
    }

    private IEnumerator SwitchSceneCoroutine(int i)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(i);
    }*/



}
