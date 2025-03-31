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
        PauseSystem.OnPauseStateChanged += action;
        if (YG2.saves.supplies[2] == 0) button.interactable = false;
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
    }

    public void Fire()
    {
        var temp = YG2.saves.supplies;
        temp[2]--;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();
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
                z.TakeDamage(damage, false);
            }
            timeLived = 0;
        }
        fired = false;
        lava.SetActive(false);
        rain.SetActive(false);
        if (YG2.saves.supplies[2] != 0) button.interactable = true;
        timeLived = 0;
        ticksPassed = 0;
    }

    private void Update()
    {
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
        if (!fired) return;
        StopCoroutine(DamageCoroutine());
        rain.GetComponent<ParticleSystem>().Pause();
    }

    private void SelfUnpause()
    {
        if (!fired) return;
        StartCoroutine(DamageCoroutine());
        rain.GetComponent<ParticleSystem>().Play();
    }
}
