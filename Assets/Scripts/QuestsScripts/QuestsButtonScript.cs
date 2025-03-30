using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestsButtonScript : MonoBehaviour
{
    public void BackToChooseGameScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(8);
    }

    public void LoadLvL_1_Scene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(4);
    }   
    public void LoadLvL_2_Scene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(5);
    }
}
