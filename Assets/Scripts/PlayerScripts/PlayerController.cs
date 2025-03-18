using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] Timer timer;
    Vector2 moveInput;
    Vector3 movement;

    private bool isSlide;

    [SerializeField] private Animator anim;
    



    [SerializeField] Weapon weapon;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        weapon.Unpause();
        EnemyZombie.OnZombieHitPlayer += OnHit;
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
        MoneySystem.SaveMoney();
        // Start Death Animation(THIS COMMENT HAS A COPY IN ENEMYSPAWNSYSTEM, WILL FIX LATER)
        Pause();
        timer.Pause();
        PauseSystem.PauseChange?.Invoke(true);
    }

    public void OnPause()
    {
        if (PauseSystem.instance.win) return;
        if (PauseSystem.instance.paused) Unpause();
        else Pause();
    }


    public void Pause()
    {
        weapon.Pause();
        anim.speed = 0;
        enabled = false;
    }

    public void Unpause()
    {
        weapon.Unpause();
        anim.speed = 1;
        enabled = true;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }


    private void FixedUpdate()
    {
        movement.x = moveInput.x * moveSpeed;
        characterController.Move(movement * Time.fixedDeltaTime);
        if (moveInput.x > 0) anim.SetInteger("MoveDirection", 1);
        else if (moveInput.x < 0) anim.SetInteger("MoveDirection", -1);
        else anim.SetInteger("MoveDirection", 0);
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
