using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    private int sec = 0;
    private int min = 0;
    private TMP_Text timerText;
    [SerializeField] private int delta = 0;


    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        StartCoroutine(ITimer());
    }

    IEnumerator ITimer()
    {
        while (true)
        {
            if (sec == 59)
            {
                min++;
                sec = -1;
            }
            sec += delta;
            timerText.text = min.ToString("D2") + " : " + sec.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }


}
