using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

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
