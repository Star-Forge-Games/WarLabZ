using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.EnhancedTouch;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float moveSpeedWithBonus = 12f;
    [SerializeField] private Animator anim;
    Vector2 moveInput = Vector2.zero;
    Vector3 movement = Vector2.zero;
    private Action<bool> action;
    private bool paused;

    public static Transform trans;

    private void Awake()
    {
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
        trans = transform;
        TouchSimulation.Enable();
        characterController = GetComponent<CharacterController>();
    }

    private void OnDestroy()
    {
        Wall.OnWallDeath -= RunAway;
        PauseSystem.OnPauseStateChanged -= action;
    }

    public void Pause(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        if (!paused)
        {
            PauseSystem.instance.Pause(false);
        }
        else
        {
            PauseSystem.instance.Unpause(false);
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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    protected Vector2 touchPosition = Vector2.zero;

    public void TouchDelta(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        touchPosition += delta;
    }

    public void TouchStart(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled) {
            touchPosition = Vector2.zero;
        }
    }

    private void Update()
    {
        if (!paused)
        {
            if (touchPosition.x != 0)
            {
                movement.x = (touchPosition.x > 0 ? 1 : -1) * moveSpeed;
                characterController.Move(movement * Time.deltaTime);
                if (touchPosition.x > 0) anim.SetInteger("MoveDirection", 1);
                else if (touchPosition.x < 0) anim.SetInteger("MoveDirection", -1);
                else anim.SetInteger("MoveDirection", 0);
            }
            else
            {
                movement.x = moveInput.x * moveSpeed;
                characterController.Move(movement * Time.deltaTime);
                if (moveInput.x > 0) anim.SetInteger("MoveDirection", 1);
                else if (moveInput.x < 0) anim.SetInteger("MoveDirection", -1);
                else anim.SetInteger("MoveDirection", 0);
            }
        }
    }

    private void RunAway()
    {
        TouchSimulation.Disable();
        anim.Play("Turn");
        paused = true;
        PauseSystem.instance.Lose();
    }

    internal void IncreaseSpeed()
    {
        moveSpeed = moveSpeedWithBonus;
    }
}
