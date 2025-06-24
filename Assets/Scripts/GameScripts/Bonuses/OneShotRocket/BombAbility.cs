using System;
using System.Collections;
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
    private bool paused = false;
    private float cooldown = 0;

    private Action<bool> action;

    private void Awake()
    {
        action = (pause =>
        {
            paused = pause;
        });
        cooldownSlider.maxValue = cooldownInSeconds;
        PauseSystem.OnPauseStateChanged += action;
    }

    private void Start()
    {
        //if (YG2.saves.supplies[1] == 0) button.interactable = false;
    }

    public void Fire()
    {
        /*var temp = YG2.saves.supplies;
        temp[1]--;
        if (temp[1] == 0) button.interactable = false;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();*/
        button.interactable = false;
        cooldown = cooldownInSeconds;
        bomber.Play("Fire");
        StartCoroutine(Drop());
    }

    private void Update()
    {
        if (paused) return;
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

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(0.4f);
        Bomb b1 = Instantiate(bombPrefab, new Vector3(3.64f, 10, 10), Quaternion.Euler(0, 0, -20));
        b1.id = 0;
        yield return new WaitForSeconds(0.2f);
        Bomb b2 = Instantiate(bombPrefab, new Vector3(3.64f, 10, 20), Quaternion.Euler(0, 0, -20));
        b2.id = 1;
        yield return new WaitForSeconds(0.2f);
        Bomb b3 = Instantiate(bombPrefab, new Vector3(3.64f, 10, 30), Quaternion.Euler(0, 0, -20));
        b3.id = 2;
        yield return new WaitForSeconds(0.2f);
        Bomb b4 = Instantiate(bombPrefab, new Vector3(3.64f, 10, 40), Quaternion.Euler(0, 0, -20));
        b4.id = 3;
        yield return new WaitForSeconds(0.2f);
        Bomb b5 = Instantiate(bombPrefab, new Vector3(3.64f, 10, 50), Quaternion.Euler(0, 0, -20));
        b5.id = 4;
        yield return new WaitForSeconds(0.2f);
        if (YG2.saves.supplies[1] != 0) button.interactable = true;
    }

}
