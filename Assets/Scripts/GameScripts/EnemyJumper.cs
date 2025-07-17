using UnityEngine;

public class EnemyJumper : EnemyZombie
{
    public new void Update()
    {
        if (dead) return;
        if (stunned) return;
        if (wall) return;
        if (transform.position.z >= slowStartZ)
        {
            wall = true;
            anim.Play("ZombieJump");
        }
        else
        {
            characterController.Move(direction * Time.deltaTime);
        }
    }
}
