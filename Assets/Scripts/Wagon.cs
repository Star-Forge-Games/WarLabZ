using System.Linq;
using UnityEngine;

public class Wagon : EnemyZombie
{

    [SerializeField] Animator bombAnimator;
    [SerializeField] ParticleSystem fire;
    [SerializeField] GameObject wagonCanvas;

    public new void Update()
    {
        if(characterController != null) 
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
        damage = (int) (Wall.instance.maxHealth * 0.1f);
        if (damage < 10) damage = 10;
        maxHealth = Weapon.instance.bulletDamage * 3;
        currentHealth = maxHealth;
        UpdateHealthUI(maxHealth, currentHealth);
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            WagonExplosion.instance.Explode(transform.position);
            Wall.instance.TakeDamage(this, -1);
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage, bool crit, bool instaKill)
    {
        currentHealth -= damage;
        if (crit)
        {
            critCanvas.SetActive(true);
            StartCoroutine(StopCanvas(true));
        }
        if (currentHealth <= 0)
        {
            dead = true;
            
            Destroy(GetComponent<CharacterController>());
            Destroy(healthCanvas);
            Destroy(wagonCanvas);
            OnZombieDie?.Invoke(this, 0, 0);
            anim.Play("Death");
            StartCoroutine(Die());
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

    public override void Stun()
    {
        return;
    }

    public override void SelfPause()
    {
        anim.speed = 0;
        bombAnimator.speed = 0;
        fire.Pause();
        enabled = false;
    }

    public override void SelfUnpause()
    {
        anim.speed = 1;
        bombAnimator.speed = 1;
        fire.Play();
        enabled = true;
    }
}
