using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LavaAbility : MonoBehaviour
{
    [SerializeField] GameObject lava, rain;
    [SerializeField] int damageTicks;
    [SerializeField] int damage;
    [SerializeField] float damageDelay;
    [SerializeField] Transform enemyContainer, wagonContainer;
    [SerializeField] Button button;
    [SerializeField] int cooldownInSeconds;
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] Slider cooldownSlider;
    private bool paused = false;
    private float cooldown = 0;
    private bool fired;
    private int ticksPassed = 0;
    private float timeAfterLastTick = 0;


    private Action<bool> action;

    public void Awake()
    {
        action = (pause =>
        {
            if (!pause) SelfUnpause();
            else SelfPause();
        });
        cooldownSlider.maxValue = cooldownInSeconds;
        PauseSystem.OnPauseStateChanged += action;
    }

    private void Start()
    {
        if (YG2.saves.supplies[2] == 0)
        {
            button.interactable = false;
            amount.text = "";
        }
        else
        {
            button.interactable = true;
            amount.text = $"{YG2.saves.supplies[1]}";
        }
    }

    public void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
    }

    public void Fire()
    {
        var temp = YG2.saves.supplies;
        temp[2]--;
        if (temp[2] == 0) button.interactable = false;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();
        amount.text = temp[2] == 0 ? "" : $"{temp[2]}";
        button.interactable = false;
        cooldown = cooldownInSeconds;
        rain.SetActive(true);
        rain.GetComponent<Animator>().Play("Acid");
        lava.SetActive(true);
        lava.GetComponent<Animator>().Play("Acid");
        fired = true;
        StartCoroutine(nameof(DamageCoroutine));
    }

    public IEnumerator DamageCoroutine()
    {
        if (ticksPassed < damageTicks)
        {
            while (ticksPassed < damageTicks)
            {
                yield return new WaitForSeconds(damageDelay - timeAfterLastTick);
                ticksPassed++;
                foreach (Transform t in enemyContainer)
                {
                    EnemyZombie z = t.GetComponent<EnemyZombie>();
                    yield return new WaitForEndOfFrame();
                    z?.TakeDamage(damage, false, false);
                }
                foreach (Transform t in wagonContainer)
                {
                    Wagon z = t.GetComponent<Wagon>();
                    yield return new WaitForEndOfFrame();
                    z?.TakeDamage(damage, false, false);
                }
                timeAfterLastTick = 0;
            }
            fired = false;
            ticksPassed = 0;
            timeAfterLastTick = 0;
            yield return new WaitForSeconds(0.5f);
        }
        lava.SetActive(false);
        rain.SetActive(false);
        fired = false;
        ticksPassed = 0;
        timeAfterLastTick = 0;
    }

    public void Update()
    {
        if (paused) return;
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldownSlider.value = cooldown;
            if (cooldown <= 0)
            {
                if (YG2.saves.supplies[2] != 0) button.interactable = true;
                cooldownSlider.value = 0;
            }
        }
        if (fired) timeAfterLastTick += Time.deltaTime;
    }

    private void SelfPause()
    {
        paused = true;
        if (!fired) return;
        StopCoroutine(nameof(DamageCoroutine));
        rain.GetComponent<ParticleSystem>().Pause();
        lava.GetComponent<Animator>().speed = 0;
        rain.GetComponent<Animator>().speed = 0;
    }

    private void SelfUnpause()
    {
        paused = false;
        if (!fired) return;
        StartCoroutine(nameof(DamageCoroutine));
        rain.GetComponent<ParticleSystem>().Play();
        lava.GetComponent<Animator>().speed = 1;
        rain.GetComponent<Animator>().speed = 1;
    }
}
