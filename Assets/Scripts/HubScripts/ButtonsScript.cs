using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{

    [SerializeField] private Animator fader;

    public void LoadShopScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }
    public void LoadQuestsScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

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
