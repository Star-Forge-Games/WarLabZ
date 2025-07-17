using UnityEngine;

public class Thrower : EnemyZombie
{
    [SerializeField] Transform throwPosition;
    [SerializeField] GameObject throwPrefab;
    private bool throwing = false;

    public new void Update()
    {
        if (characterController == null) return;
        if (throwing) return;
        if (SkillsPanel.zombieSlow)
        {
            if (transform.position.z <= slowStartZ && transform.position.z >= slowEndZ)
            {
                direction.z = -speed * 0.6f;
                slowCanvas.SetActive(true);
            }
            else
            {
                direction.z = -speed;
            }
        }
        else
        {
            slowCanvas.SetActive(false);
        }
        if (transform.position.z <= 30)
        {
            throwing = true;
            anim.SetBool("Throwing", true);
            slowCanvas.SetActive(false);
            return;
        }
        characterController.Move(direction * Time.deltaTime);
    }

    public override void Attack()
    {
        GameObject g = Instantiate(throwPrefab, throwPosition.position, Quaternion.identity);
        g.transform.SetParent(StoneContainer.trans);
        ThrowObject o = g.GetComponent<ThrowObject>();
        o.Setup(this, damage);
    }
}
