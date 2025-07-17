using UnityEngine;

public class AnimUtility : MonoBehaviour
{
    private EnemyZombie z;

    private void Start()
    {
        z = GetComponentInParent<EnemyZombie>();
    }

    public void Attack()
    {
        z.Attack();
    }

    public void JumpAttack()
    {
        ((EnemyJumper)z).JumpAttack();
    }

    public void TurnInvincible()
    {
        ((EnemyJumper)z).TurnInvincible();
    }
}
