using UnityEngine;

public class WagonExplosion : MonoBehaviour
{

    public static WagonExplosion instance;

    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Explode(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        GetComponent<ParticleSystem>().Play();
    }

}
