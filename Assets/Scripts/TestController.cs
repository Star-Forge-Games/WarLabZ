using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;


public class TestController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 dir;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;

    private bool isSlide;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            // transform.position += new Vector3(-4f, 0, 0) * Time.deltaTime;    
            dir.x = speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            // transform.position += new Vector3(4f, 0, 0) * Time.deltaTime;
            dir.x = -speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Slide());
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("Up");
            if (controller.isGrounded)
                Jump();
        }
    }

    private void Jump()
    {
        dir.y = jumpForce;
    }

    void FixedUpdate()
    {
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private IEnumerator Slide()
    {
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1f;
        isSlide = true;


        yield return new WaitForSeconds(0.9f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        isSlide = false;

    }

}
