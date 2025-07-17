using UnityEngine;

public class AbotUI : MonoBehaviour
{
    [SerializeField] GameObject skilslPanel;
    [SerializeField] GameObject controlsPanel;

    private void OnEnable()
    {
        ShowPanel(0);
    }
    public void ShowPanel(int i)
    {

        skilslPanel.SetActive(i == 0);
        controlsPanel.SetActive(i == 1);


    }
}
