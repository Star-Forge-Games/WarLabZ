using System;
using UnityEngine;

public class WagonSignal : MonoBehaviour
{

    [SerializeField] AudioSource signal;
    private Animator anim;

    private Action<bool> action;

    private void Start()
    {

        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
        action = p =>
        {
            if (p) SelfPause();
            else SelfUnpause();
        };
        PauseSystem.OnPauseStateChanged += action;
    }

    public void Signal()
    {
        gameObject.SetActive(true);
        signal.Play();
        anim.Play("Signalize");
    }

    public void SelfPause()
    {
        signal.Pause();
        anim.speed = 0;
    }

    public void SelfUnpause()
    {
        signal.UnPause();
        anim.speed = 1;
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
    }


}
