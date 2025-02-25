using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestsButtonScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadQuestsScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }

}
