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

    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float jumpGravity = 0.75f;
    bool isJumping = false;

    private bool isSlide;

    public GameObject bulletPrefab;// префаб снаряда
    public Transform firePoint; // откуда стреляем
    [SerializeField] float fireForce; // Скорость полёта снаряда
    [SerializeField] float bulletsRate; // Частота выстерлов

    [SerializeField] private Animator anim;
    [SerializeField] private Transform bulletContainer;

    public static int money;
    [SerializeField] private TMP_Text moneyText;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        StartCoroutine(nameof(PeriodicFireSpawn));
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
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.Save();
        // Start Death Animation
        StopCoroutine(nameof(PeriodicFireSpawn));
        anim.speed = 0;
        enabled = false;
        foreach (Transform bullet in bulletContainer)
        {
            bullet.GetComponent<Bullet>().Stop();
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (characterController.isGrounded && !isJumping) isJumping = true;
    }

    private void FixedUpdate()
    {
        movement.x = moveInput.x * moveSpeed;

        if (characterController.isGrounded)
        {
            movement.y = -jumpGravity * 0.1f;

            if (isJumping)
            {
                movement.y = jumpSpeed;
                isJumping = false;
            }
        }
        else
        {
            movement.y -= jumpGravity * (movement.y > 0 ? 1 : 1.25f);
        }
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

    private IEnumerator PeriodicFireSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(bulletsRate);
            if (characterController.isGrounded)
                Fire();
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //создаём снаряд из префаба
        bullet.transform.parent = bulletContainer;
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>(); //получаем rigidbody для префаба 

        bulletRB.AddForce(firePoint.forward * fireForce, ForceMode.Impulse); //применяем силу к Rigidbody снаряда, чтобы запустить снаряд
    }

    /* private void OnControllerColliderHit(ControllerColliderHit hit)
     {

         if (hit.gameObject.tag == "obstacle")
         {
            Debug.Log("ZHopa");
            losePanel.SetActive(true);
             Time.timeScale = 0;
         }
     }*/

}
