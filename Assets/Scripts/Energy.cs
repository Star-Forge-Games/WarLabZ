using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Energy : MonoBehaviour
{

    [SerializeField] Button playButton;
    [SerializeField] Slider energySlider;
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] int energyGainPerAd = 3;
    [SerializeField] GameObject adPanel;
    private int energy;
    [SerializeField] private int maxEnergy = 15;
    [SerializeField] private int energyRechargeIntervalInSeconds;
    private long energyRechargeInterval;
    private long secsLeft;

    private void OnEnable()
    {
        energySlider.maxValue = maxEnergy;
        if (!YG2.saves.playedBefore)
        {
            YG2.saves.playedBefore = true;
            YG2.saves.energyLeft = maxEnergy;
            energy = maxEnergy;
            YG2.SaveProgress();
            UpdateEnergySlider(false, energy);
            playButton.interactable = true;
            return;
        }
        energyRechargeInterval = (long)energyRechargeIntervalInSeconds * 10000000;
        energy = YG2.saves.energyLeft;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
            YG2.saves.energyLeft = energy;
            YG2.SaveProgress();
        }
        if (energy == maxEnergy)
        {
            UpdateEnergySlider(false, energy);
            playButton.interactable = true;
            return;
        }
        long ts = YG2.saves.lastEnergySpentTimeStamp;
        long cts = DateTime.Now.Ticks;
        while (ts <= cts - energyRechargeInterval)
        {
            energy++;
            cts -= energyRechargeInterval;
            if (energy == maxEnergy) break;
        }
        if (cts > ts) cts -= ts;
        YG2.saves.energyLeft = energy;
        YG2.SaveProgress();
        UpdateEnergySlider(false, energy);
        playButton.interactable = true;
        if (energy == maxEnergy) return;
        long secondsLeft = (energyRechargeInterval - cts) / 10000000;
        secsLeft = secondsLeft;
        if (energy == 0)
        {
            playButton.interactable = false;
            UpdateEnergySlider(true, secondsLeft);
            StartCoroutine(nameof(TimerTick));
        }
        else
        {
            UpdateEnergySlider(false, energy);
            playButton.interactable = true;
        }
        StartCoroutine(RechrageEnergy(secondsLeft));
    }

    private IEnumerator RechrageEnergy(long seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopCoroutine(nameof(TimerTick));
        if (energy != maxEnergy) energy++;
        YG2.saves.energyLeft = energy;
        YG2.SaveProgress();
        UpdateEnergySlider(false, energy);
        playButton.interactable = true;
        while (true)
        {
            yield return new WaitForSeconds(energyRechargeIntervalInSeconds);
            if (energy != maxEnergy)
            {
                energy++;
                StopCoroutine(nameof(TimerTick));
                YG2.saves.energyLeft = energy;
                YG2.saves.lastEnergySpentTimeStamp = DateTime.Now.Ticks;
                YG2.SaveProgress();
                UpdateEnergySlider(false, energy);
                playButton.interactable = true;
            }
        }
    }

    private void UpdateEnergySlider(bool zero, long number)
    {
        if (!zero)
        {
            energySlider.value = number;
            energyText.text = $"{number}";
        }
        else
        {
            energySlider.value = 0;
            long m = number / 60;
            long s = number % 60;
            energyText.text = (m > 9 ? m : $"0{m}") + ":" + (s > 9 ? s : $"0{s}");
        }
    }

    private IEnumerator TimerTick()
    {
        while (secsLeft > 0)
        {
            yield return new WaitForSeconds(1);
            secsLeft--;
            if (secsLeft > 0)
            {
                UpdateEnergySlider(true, secsLeft);
            }
        }
    }

    public void PlayAD()
    {
        if (energy == maxEnergy) return;
        adPanel.SetActive(true);
        gameObject.SetActive(false);
        AudioListener.volume = 0;
        YG2.RewardedAdvShow("1", () => AddEnergyByAd());
    }


    public void AddEnergyByAd()
    {
        adPanel.SetActive(false);
        gameObject.SetActive(true);
        energy = Mathf.Clamp(energy + energyGainPerAd, 0, maxEnergy);
        YG2.saves.energyLeft = energy;
        YG2.SaveProgress();
        StopCoroutine(nameof(TimerTick));
        UpdateEnergySlider(false, energy);
        playButton.interactable = true;
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
    }

    internal void Spend()
    {
        if (energy == maxEnergy)
        {
            YG2.saves.lastEnergySpentTimeStamp = DateTime.Now.Ticks;
        }
        energy--;
        YG2.saves.energyLeft = energy;
        YG2.SaveProgress();
    }
}
