using UnityEngine;

public class BombSystem : MonoBehaviour
{
    public static BombSystem instance;
    [SerializeField] float interval;
    private float timer;
    private bool paused = false;
    [SerializeField] ParticleSystem timedPS, pointPS;
    [SerializeField] SkillBomb bomb;
    private bool bombFalling;

    private void Awake()
    {
        instance = this;
        enabled = false;
        PauseSystem.OnPauseStateChanged += b => paused = b;
    }

    public void Enable()
    {
        enabled = true;
        timer = 0;
    }

    private void Update()
    {
        if (!paused) timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;
            Throw();
        }
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= b => paused = b;
    }

    public void Throw()
    {
        float z = Random.Range(1, 4.5f) * 10;
        float x = Random.Range(-4.5f, 4.5f);
        timedPS.transform.position = new Vector3(x, 0, z);
        SkillBomb sb = Instantiate(bomb, new Vector3(x, 10, z), Quaternion.identity);
        sb.Setup(true);
    }

    public void ThrowAt(float x, float z)
    {
        if (bombFalling) return;
        pointPS.transform.position = new Vector3(x, 0, z-1);
        SkillBomb sb = Instantiate(bomb, new Vector3(x, 10, z-1), Quaternion.identity);
        bombFalling = true;
        sb.Setup(false);
    }

    public void Explode(bool timed)
    {
        if (timed)
        {
            timedPS.Play();
            timedPS.GetComponent<AudioSource>().Play();
        }
        else
        {
            pointPS.Play();
            pointPS.GetComponent<AudioSource>().Play();
            bombFalling = false;
        }
    }
}
