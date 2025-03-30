using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private Animator fader;
    [SerializeField] private TextMeshProUGUI cash, donateCash, keys;

    private void Start()
    {
        cash.text = $"$ {YG2.saves.cash}";
        donateCash.text = $"(G) {YG2.saves.donateCash}";
        keys.text = $"()--E {YG2.saves.keys}";
    }

    public void LoadShopScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
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

   /* public void LoadQuestsScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }*/

}
