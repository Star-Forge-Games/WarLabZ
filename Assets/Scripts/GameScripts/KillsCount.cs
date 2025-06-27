using UnityEngine;
using TMPro;

public class KillsCount : MonoBehaviour
{
    
    TMP_Text text;
    public static int kills;
    public static int bosses;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = "Kills: " + kills;
    }
}
