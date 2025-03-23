using UnityEngine;
using UnityEngine.SceneManagement;

public class WallShopButtonScript : MonoBehaviour
{

    [SerializeField] private GameObject lvl1Panel;
    [SerializeField] private GameObject lvl2Panel;
    [SerializeField] private GameObject lvl3Panel;
    [SerializeField] private GameObject lvl4Panel;
    [SerializeField] private GameObject lvl5Panel;
    [SerializeField] private GameObject lvl6Panel;
    [SerializeField] private GameObject lvl7Panel;
    [SerializeField] private GameObject lvl8Panel;
    [SerializeField] private GameObject lvl9Panel;
    [SerializeField] private GameObject lvl10Panel;
    [SerializeField] private GameObject lvl11Panel;
    [SerializeField] private GameObject lvl12Panel;




    private void Start()
    {
        lvl1Panel.SetActive(true);//Должно быть чтобы подгружалось из сейвов и запускало ту, которая у игрока
        Time.timeScale = 1;
    }

    public void BackToShopScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }
    public void LvL1PanelController()
    {
        lvl1Panel.SetActive(true);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL2PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(true);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL3PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(true);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL4PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(true);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL5PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(true);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL6PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(true);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL7PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(true);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL8PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(true);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL9PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(true);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL10PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(true);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(false);
    }

    public void LvL11PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(true);
        lvl12Panel.SetActive(false);
    }

    public void LvL12PanelController()
    {
        lvl1Panel.SetActive(false);
        lvl2Panel.SetActive(false);
        lvl3Panel.SetActive(false);
        lvl4Panel.SetActive(false);
        lvl5Panel.SetActive(false);
        lvl6Panel.SetActive(false);
        lvl7Panel.SetActive(false);
        lvl8Panel.SetActive(false);
        lvl9Panel.SetActive(false);
        lvl10Panel.SetActive(false);
        lvl11Panel.SetActive(false);
        lvl12Panel.SetActive(true);
    }

}
