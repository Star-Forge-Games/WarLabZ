using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] float moveSpeed = 6f;
    Vector2 moveInput;
    Vector3 movement;

    //[SerializeField] private GameObject losePanel;

    private bool isSlide;

    [SerializeField] private Animator anim;
    

    public static int money;
    [SerializeField] private TMP_Text moneyText;

    [SerializeField] Weapon weapon;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        weapon.Unpause();
        EnemyZombie.OnZombieHitPlayer += OnHit;
        money = PlayerPrefs.GetInt("money");
        moneyText.text = "$: " + money;
    }

    private void OnDestroy()
    {
        EnemyZombie.OnZombieHitPlayer -= OnHit;
    }

    void Update()
    {
        moneyText.text = "$: " + money;
        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Slide());
        }
    }

    private void OnHit()
    {
        SaveMoney();
        // Start Death Animation
        Pause();
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.Save();
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

    void OnMove(InputValue value)
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
