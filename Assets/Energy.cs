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
    private int energy;
    [SerializeField] private int maxEnergy = 15;
    [SerializeField] private int energyRechargeIntervalInSeconds;
    private long energyRechargeInterval;
    private int secsLeft;

    private void Start()
    {
        energySlider.maxValue = maxEnergy;
        Debug.Log("Played before: " + YG2.saves.playedBefore);
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
        energyRechargeInterval = (long) energyRechargeIntervalInSeconds * 10000000;
        energy = YG2.saves.energyLeft;
        Debug.Log("Energy: " + YG2.saves.energyLeft);
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
        int secondsLeft = (int) (cts / 10000000);
        Debug.Log("secondsLeft = " + secondsLeft);
        secsLeft = secondsLeft;
        if (energy == 0)
        {
            playButton.interactable = false;
            UpdateEnergySlider(true, secondsLeft);
            StartCoroutine(nameof(TimerTick));
        } else
        {
            UpdateEnergySlider(false, energy);
            playButton.interactable = true;
        }
        StartCoroutine(RechrageEnergy(secondsLeft));
    }

    private IEnumerator RechrageEnergy(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        energy++;
        StopCoroutine(nameof(TimerTick));
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
                YG2.SaveProgress();
                UpdateEnergySlider(false, energy);
                playButton.interactable = true;
            }
        }
    }

    private void UpdateEnergySlider(bool zero, int number)
    {
        if (!zero)
        {
            energySlider.value = number;
            energyText.text = $"{number}";
        } else
        {
            energySlider.value = 0;
            int m = number / 60;
            int s = number % 60;
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

    public void RechargeEnergyByAd()
    {
        if (energy == maxEnergy) return;
        energy = Mathf.Clamp(energy + energyGainPerAd, 0, maxEnergy);
        YG2.saves.energyLeft = energy;
        YG2.SaveProgress();
        StopCoroutine(nameof(TimerTick));
        UpdateEnergySlider(false, energy);
        playButton.interactable = true;
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
