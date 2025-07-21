using System;
using System.Linq;
using UnityEngine;

public class EnemyJumper : EnemyZombie
{

    private bool invincible;

    public new void Update()
    {
        if (dead) return;
        if (stunned) return;
        if (wall) return;
        if (!invincible) return;
        characterController.Move(direction * Time.deltaTime);
    }

    public new void Start()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            var smr = parts[i].obj.GetComponent<SkinnedMeshRenderer>();
            parts[i].colors = (from m in smr.materials select m.color).ToArray();
        }
        direction.z = -speed;
        characterController = GetComponent<CharacterController>();
        if (boss)
        {
            if (SkillsPanel.bossHealthReduction)
            {
                maxHealth = (int)(maxHealth * 0.95f);
                currentHealth = maxHealth;
            }
        }
        else
        {
            if (SkillsPanel.zHealthReduction)
            {
                maxHealth = (int)(maxHealth * 0.975f);
                currentHealth = maxHealth;
            }
        }
        UpdateHealthUI(maxHealth, currentHealth);
        Wall.OnWallDeath += RunFurther;
        anim.Play("ZombieJump");
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            wall = true;
            if (invincible)
            {
                Wall.instance.TakeDamage(this, damage);
            }
        }
    }

    public override void TakeDamage(int damage, bool crit, bool instaKill)
    {
        if (invincible) return;
        base.TakeDamage(damage, crit, instaKill);
    }

    public void JumpAttack()
    {
        if (!wall) return;
        anim.Play("Attack");
        invincible = false;
    }

    public override void Stun()
    {
        return;
    }

    internal void TurnInvincible()
    {
        invincible = !invincible;
    }
}
