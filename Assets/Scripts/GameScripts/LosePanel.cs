using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void BackToHub()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }



}
