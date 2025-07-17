using UnityEngine;

public class WagonExplosion : MonoBehaviour
{

    public static WagonExplosion instance;

    [SerializeField] AudioSource sound;
    [SerializeField] ParticleSystem ps;

    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Explode(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        ps.Play();
        sound.Play();
    }

}
