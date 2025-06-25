using System;
using System.Collections;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BombAbility : MonoBehaviour
{

    [SerializeField] Bomb bombPrefab;
    [SerializeField] Animator bomber;
    [SerializeField] Button button;
    [SerializeField] int cooldownInSeconds;
    [SerializeField] Slider cooldownSlider;
    [SerializeField] TextMeshProUGUI amount;
    private bool paused = false;
    private float cooldown = 0;
    private float delay;
    private bool fired = false;
    private int bombsDropped = 0;

    private Action<bool> action;

    private void Awake()
    {
        action = (pause =>
        {
            if (pause) SelfPause();
            else SelfUnpause();
        });
        cooldownSlider.maxValue = cooldownInSeconds;
        PauseSystem.OnPauseStateChanged += action;
    }

    private void Start()
    {
        if (YG2.saves.supplies[1] == 0)
        {
            button.interactable = false;
            amount.text = "";
        } else
        {
            button.interactable = true;
            amount.text = $"{YG2.saves.supplies[1]}";
        }
    }

    public void Fire()
    {
        var temp = YG2.saves.supplies;
        temp[1]--;
        if (temp[1] == 0) button.interactable = false;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();
        amount.text = temp[1] == 0 ? "" : $"{temp[1]}";
        button.interactable = false;
        cooldown = cooldownInSeconds;
        fired = true;
        bomber.Play("Fire");
        StartCoroutine(nameof(Drop), delay);
    }

    private void Update()
    {
        if (paused) return;
        if (fired)
        {
            if (bombsDropped == 0) delay = Mathf.Clamp(delay + Time.deltaTime, 0, 1.4f);
            else delay = Mathf.Clamp(delay + Time.deltaTime, 0, 0.2f);
        }
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldownSlider.value = cooldown;
            if (cooldown <= 0)
            {
                if (YG2.saves.supplies[1] != 0) button.interactable = true;
                cooldownSlider.value = 0;
            }
        }
    }

    private void SelfPause()
    {
        paused = true;
        if (fired)
        {
            StopCoroutine(nameof(Drop));
            bomber.speed = 0;
        }
    }

    private void SelfUnpause()
    {
        paused = false;
        if (fired)
        {
            bomber.speed = 1;
            if (bombsDropped < 5)
            {
                StartCoroutine(nameof(Drop), delay);
            } 
        }
    }

    private IEnumerator Drop(float delay)
    {
        if (bombsDropped == 0 && delay < 1.4f) yield return new WaitForSeconds(1.4f - delay);
        if (bombsDropped == 0)
        {
            while (bombsDropped < 5)
            {
                Bomb b = Instantiate(bombPrefab, new Vector3(3.64f, 10, 8.5f + 10 * bombsDropped), Quaternion.Euler(0, 0, -20));
                b.id = bombsDropped;
                bombsDropped++;
                if (bombsDropped != 5)
                {
                    yield return new WaitForSeconds(0.2f);
                } else
                {
                    fired = false;
                    bombsDropped = 0;
                    delay = 0;
                    break;
                }
            }
        } else
        {
            yield return new WaitForSeconds(0.2f - delay);
            while (bombsDropped < 5)
            {
                Bomb b = Instantiate(bombPrefab, new Vector3(3.64f, 10, 8.5f + 10 * bombsDropped), Quaternion.Euler(0, 0, -20));
                b.id = bombsDropped;
                bombsDropped++;
                if (bombsDropped != 5)
                {
                    yield return new WaitForSeconds(0.2f);
                } else
                {
                    fired = false;
                    bombsDropped = 0;
                    delay = 0;
                    break;
                }
            }
        }  
    }

    private void OnDestroy()
    {
        PauseSystem.OnPauseStateChanged -= action;
    }

}
