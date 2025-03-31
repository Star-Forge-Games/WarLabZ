using UnityEngine;

public class ExplosionTestOptimization : MonoBehaviour
{
    [SerializeField] ParticleSystem[] explosions;

    public static ExplosionTestOptimization instance;
    private void Awake()
    {
        instance = this;
    }

    public void Activate(int index)
    {
        explosions[index].Play();
    }
}
