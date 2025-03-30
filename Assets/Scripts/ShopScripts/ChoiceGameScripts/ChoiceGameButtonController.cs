using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceGameButtonController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToHubScene()
    {
        SceneManager.LoadScene(0);
        // fader
    }

    public void RunStoryModeScene()
    {
        SceneManager.LoadScene(2);
        // fader
    }


}
