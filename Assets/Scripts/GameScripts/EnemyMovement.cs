using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 dir;

    [SerializeField] private float speed;
    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        dir.z = -speed;
        characterController.Move(dir * Time.deltaTime);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            Time.timeScale = 0;
            // add invoked action to stop zombies + anims
            //GetComponent<Animator>().speed = 0;
            //this.enabled = false;
        }
    }

}
