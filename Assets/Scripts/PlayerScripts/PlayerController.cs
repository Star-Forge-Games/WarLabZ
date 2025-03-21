using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform weapons;
    private Weapon weapon;
    Vector2 moveInput;
    Vector3 movement;
    private bool isSlide;
    private Action<bool> action;
    private bool paused;

    private void Awake()
    {
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        PauseSystem.OnPauseStateChanged += action;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        EnemyZombie.OnZombieHitPlayer += OnHit;
        weapon = weapons.GetComponentInChildren<Weapon>();
        weapon.SelfUnpause();
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitPlayer -= OnHit;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Slide());
        }
    }

    private void OnHit()
    {
        PauseSystem.instance.Lose();
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

    private IEnumerator Slide()
    {
        characterController.center = new Vector3(0, -0.5f, 0);
        characterController.height = 1f;
        isSlide = true;
        yield return new WaitForSeconds(0.9f);
        characterController.center = new Vector3(0, 0, 0);
        characterController.height = 2;
        isSlide = false;
    }


}
