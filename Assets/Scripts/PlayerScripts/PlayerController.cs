using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController characterController;
    [SerializeField] float moveSpeed = 6f;
    Vector2 moveInput;
    Vector3 movement;

    [SerializeField] private GameObject losePanel;

    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float jumpGravity = 0.75f;
    bool isJumping = false;

    private bool isSlide;

    public GameObject bulletPrefab;// ������ �������
    public Transform firePoint; // ������ ��������
    [SerializeField] float fireForce; // �������� ����� �������
    [SerializeField] float bulletsRate; // ������� ���������



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Time.timeScale = 1;
        StartCoroutine(PeriodicFireSpawn());
    }


    void Update()
    {
        if (Time.timeScale == 0)
        {
            losePanel.SetActive(true);
        }

        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Slide());
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //������ ������ �� �������
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>(); //�������� rigidbody ��� ������� 

        bulletRB.AddForce(firePoint.forward * fireForce, ForceMode.Impulse); //��������� ���� � Rigidbody �������, ����� ��������� ������

        Destroy(bullet, 3f);

    }

}
