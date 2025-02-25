using UnityEngine;
using TMPro;

public class KillsCount : MonoBehaviour
{
    
    TMP_Text text;

    public static int kills;
    void Start()
    {
        text = GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Kills: " + kills;
    }
}
