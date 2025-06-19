using System.Linq;
using UnityEngine;

public class Wagon : EnemyZombie
{

    public new void Update()
    {
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
        MultiplyHp(1);
        UpdateHealthUI(maxHealth, currentHealth);
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            WagonExplosion.instance.Explode(transform.position);
            Wall.instance.TakeDamage(this, damage);
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage, bool crit, bool instaKill)
    {
        currentHealth -= damage;
        if (crit)
        {
            // визуал крита
        }
        if (currentHealth <= 0)
        {
            dead = true;
            Destroy(GetComponent<CharacterController>());
            Destroy(healthCanvas);
            OnZombieDie?.Invoke(this, 0, 0);
            anim.Play("Death");
            StartCoroutine(Die());
            Destroy(gameObject);
        }
        else
        {
            foreach (var part in parts)
            {
                foreach (var mat in part.obj.GetComponent<SkinnedMeshRenderer>().materials)
                {
                    mat.color = Color.red;
                }
            }
            StartCoroutine(StopFlash());
            UpdateHealthUI(maxHealth, currentHealth);
        }
    }
}
