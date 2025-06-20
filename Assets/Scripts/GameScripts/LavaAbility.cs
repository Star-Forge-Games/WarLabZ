using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LavaAbility : MonoBehaviour
{
    [SerializeField] GameObject lava, rain;
    [SerializeField] int damageTicks;
    [SerializeField] int damage;
    [SerializeField] float damageDelay;
    [SerializeField] Transform enemyContainer;
    [SerializeField] Button button;
    [SerializeField] int cooldownInSeconds;
    [SerializeField] Slider cooldownSlider;
    private bool paused = false;
    private float cooldown = 0;
    private float timeLived;
    private float ticksPassed;
    private bool fired;


    private Action<bool> action;

    private void Awake()
    {
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        cooldownSlider.maxValue = cooldownInSeconds;

        PauseSystem.OnPauseStateChanged += action;
        //if (YG2.saves.supplies[2] == 0) button.interactable = false;
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
    }

    public void Fire()
    {
        /*var temp = YG2.saves.supplies;
        temp[2]--;
        if (temp[2] == 0) button.interactable = false;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();*/
        button.interactable = false;
        cooldown = cooldownInSeconds;
        fired = true;
        rain.SetActive(true);
        StartCoroutine(DamageCoroutine());
        StartCoroutine(WetFloor());
    }

    public IEnumerator DamageCoroutine()
    {
        for (int i = 0; i < damageTicks - ticksPassed; i++)
        {
            yield return new WaitForSeconds(damageDelay - timeLived);
            ticksPassed++;
            foreach (Transform t in enemyContainer)
            {
                EnemyZombie z = t.GetComponent<EnemyZombie>();
                yield return new WaitForEndOfFrame();
                if (z != null)
                z.TakeDamage(damage, false, false);
            }
            timeLived = 0;
        }
        fired = false;
        lava.SetActive(false);
        rain.SetActive(false);
        //if (YG2.saves.supplies[2] != 0) button.interactable = true;
        timeLived = 0;
        ticksPassed = 0;
    }

    private void Update()
    {
        if (!paused)
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                cooldownSlider.value = cooldown;
                if (cooldown <= 0)
                {
                    button.interactable = true;
                    cooldownSlider.value = 0;
                }
            }
        }
        if (fired)
        {
            timeLived += Time.deltaTime;
        }
    }

    public IEnumerator WetFloor()
    {
        yield return new WaitForSeconds(1);
        lava.SetActive(true);
    }

    private void SelfPause()
    {
        paused = true;
        if (!fired) return;
        StopCoroutine(DamageCoroutine());
        rain.GetComponent<ParticleSystem>().Pause();
    }

    private void SelfUnpause()
    {
        paused = false;
        if (!fired) return;
        StartCoroutine(DamageCoroutine());
        rain.GetComponent<ParticleSystem>().Play();
    }
}
