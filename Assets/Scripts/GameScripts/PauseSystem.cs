using UnityEngine;

public class PauseSystem : MonoBehaviour
{

    private static PauseSystem instance;

    private void Awake()
    {
        instance = this;
    }

    public static void Pause()
    {
        
    }

    public static void Unpause()
    {

    }

}
