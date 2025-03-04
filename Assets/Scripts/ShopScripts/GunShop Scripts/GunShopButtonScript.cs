using UnityEngine;

public class GunShopButtonScript : MonoBehaviour
{

    [SerializeField] private GameObject pistolsPanel;
    [SerializeField] private GameObject smgPanel;
    [SerializeField] private GameObject arPanel;
    [SerializeField] private GameObject legendaryPanel;

    private void Start()
    {
        pistolsPanel.SetActive(true);
        Time.timeScale = 1;
    }
    public void pistolsPanelController()
    {
        pistolsPanel.SetActive(true);
        smgPanel.SetActive(false);
        arPanel.SetActive(false);
        legendaryPanel.SetActive(false);
    }   
    
    public void smgPanelController()
    {
        smgPanel.SetActive(true);
        pistolsPanel.SetActive(false);
        arPanel.SetActive(false);
        legendaryPanel.SetActive(false);
    }

    public void arPanelController()
    {
        arPanel.SetActive(true);
        smgPanel.SetActive(false);
        pistolsPanel.SetActive(false);
        legendaryPanel.SetActive(false);
    }

    public void legendaryPanelController()
    {
        legendaryPanel.SetActive(true);
        arPanel.SetActive(false);
        smgPanel.SetActive(false);
        pistolsPanel.SetActive(false);
    }







}
