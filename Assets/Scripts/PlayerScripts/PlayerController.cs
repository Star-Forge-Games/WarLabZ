using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using YG;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float moveSpeedWithBonus = 12f;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource sound;
    Vector2 moveInput = Vector2.zero;
    Vector3 movement = Vector2.zero;
    private Action<bool> action;
    private bool paused;
    private bool touching;
    private float avgFps;
    private int fpsCounter;

    public static Transform trans;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
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
        characterController = GetComponent<CharacterController>();
    }

    public void Shot()
    {
        sound.Play();
    }

    private void OnDestroy()
    {
        Wall.OnWallDeath -= RunAway;
        PauseSystem.OnPauseStateChanged -= action;
    }

    public void Pause(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        if (!enabled) return;
        if (!paused)
        {
            PauseSystem.instance.Pause(false);
        }
        else
        {
            PauseSystem.instance.Unpause(false);
        }
    }

    public void Unpause()
    {
        PauseSystem.instance.Unpause(false);
    }

    public void Pause()
    {
        if (!enabled) return;
        PauseSystem.instance.Pause(false);
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
        if (!touching) return;
        Vector2 delta = context.ReadValue<Vector2>();
        touchPosition += delta;
    }

    public void TouchStart(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            touchPosition = Vector2.zero;
        }
        touching = !context.canceled;
    }

    private void UpdateRenderScale()
    {
        fpsCounter++;
        float fps = 1 / Time.deltaTime;
        if (fpsCounter == 100)
        {
            var urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
            fpsCounter = 0;
            avgFps -= 5;
            if (avgFps >= 60)
            {
                urp.renderScale = Mathf.Clamp(urp.renderScale + 0.05f, 0.5f, 1f);
            } else
            {
                urp.renderScale = Mathf.Clamp(urp.renderScale - 0.05f, 0.5f, 1f);
            }
            avgFps = fps;
            return;
        }
        avgFps = (avgFps + fps) / 2;
    }

    private void Update()
    {
        UpdateRenderScale();
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
        if (!YG2.envir.isMobile)
        {
            TouchSimulation.Disable();
        }
        anim.Play("Turn");
        paused = true;
        PauseSystem.instance.Lose();
    }

    internal void IncreaseSpeed()
    {
        moveSpeed = moveSpeedWithBonus;
    }
}
