using System.Collections;
using UnityEngine;

public class WagonSystem : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private float interval;
    private float timer = 0;
    private bool paused = false;

    void Start()
    {
        PauseSystem.OnPauseStateChanged += Pause;
        StartCoroutine(Cart());
    }

    private void Update()
    {
        if (paused) return;
        timer += Time.deltaTime;
    }

    private IEnumerator Cart()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval - timer);
            Instantiate(prefab, new Vector3(Random.Range(-4f, 4f), 1, 60), Quaternion.Euler(0, 180, 0));
            timer = 0;
        }
    }

    private void Pause(bool pause)
    {
        if (pause)
        {
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(Cart());
        }
        paused = pause;
    }

    public void OnDestroy()
    {
       PauseSystem.OnPauseStateChanged -= Pause;
    }

}
