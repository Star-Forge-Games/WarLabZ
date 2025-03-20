using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private float time = 0;
    private bool paused;
    private TMP_Text timerText;

    private Action<bool> action;

    private void Awake()
    {
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
    }

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (!paused) time += Time.deltaTime;
        int timeSeconds = (int)time;
        int mins = timeSeconds / 60;
        int secs = timeSeconds % 60;
        timerText.text = (mins > 9 ? mins : "0" + mins) + " : " + (secs > 9 ? secs : "0" + secs);
    }

    public void SelfPause()
    {
        paused = true;
    }

    public void SelfUnpause()
    {
        paused = false;
    }

}
