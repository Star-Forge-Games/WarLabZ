using UnityEngine;

public class LineTest : MonoBehaviour
{
    private LineRenderer lr;
    private float threshold;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);

    }

    void Update()
    {
        lr.SetPosition(1, transform.position);
        if (Vector3.Distance(lr.GetPosition(0), transform.position) > threshold)
        {
            lr.SetPosition(0, transform.position - transform.forward * threshold);
        }
    }

    public void Setup(float rate)
    {
        float b = Mathf.Log(0.2f) / Mathf.Log(100);
        threshold = 3 * (Mathf.Pow(rate, b));
    }
}
