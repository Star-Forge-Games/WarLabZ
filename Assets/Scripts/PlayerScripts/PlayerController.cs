using UnityEngine;
using UnityEngine.InputSystem;
using System;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float moveSpeedWithBonus = 12f;
    [SerializeField] private Animator anim;
    Vector2 moveInput;
    Vector3 movement;
    private Action<bool> action;
    private bool paused;

    public static Transform trans;

    private void Awake()
    {
        trans = transform;
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        Wall.OnWallDeath += RunAway;
        PauseSystem.OnPauseStateChanged += action;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnDestroy()
    {
        Wall.OnWallDeath -= RunAway;
        PauseSystem.OnPauseStateChanged -= action;
    }

    public void OnPause()
    {
        if (!paused)
        {
            PauseSystem.instance.Pause(false);
        } else
        {
            PauseSystem.instance.Unpause();
        }
    }
    public void SelfPause()
    {
        anim.speed = 0;
        paused = true;
    }

    public void SelfUnpause()
    {
        anim.speed = 1;
        paused = false;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }


    private void FixedUpdate()
    {
        if (!paused)
        {
            movement.x = moveInput.x * moveSpeed;
            characterController.Move(movement * Time.fixedDeltaTime);
            if (moveInput.x > 0) anim.SetInteger("MoveDirection", 1);
            else if (moveInput.x < 0) anim.SetInteger("MoveDirection", -1);
            else anim.SetInteger("MoveDirection", 0);   
        }
    }

    private void RunAway()
    {
        anim.Play("Turn");
        paused = true;
        PauseSystem.instance.Lose();
    }

    internal void IncreaseSpeed()
    {
        moveSpeed = moveSpeedWithBonus;
    }
}
