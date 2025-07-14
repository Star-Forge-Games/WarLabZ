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
    [SerializeField] private int maxEnergy = 15;
    [SerializeField] private int energyRechargeIntervalInSeconds;

    private void OnEnable()
    {
        energySlider.maxValue = maxEnergy;
        if (YG2.saves.playedBefore <= 0)
        {
            YG2.saves.energyLeft = maxEnergy;
            YG2.saves.nextEnergyRechargeTimeStamp = 0;
            YG2.SaveProgress();
            UpdateEnergySlider(false, maxEnergy);
            playButton.interactable = true;
            return;
        }
        if (YG2.saves.energyLeft >= maxEnergy)
        {
            YG2.saves.energyLeft = maxEnergy;
            YG2.saves.nextEnergyRechargeTimeStamp = 0;
            YG2.SaveProgress();
            UpdateEnergySlider(false, maxEnergy);
            playButton.interactable = true;
            return;
        }
        StartupRecharge();
    }

    private void StartupRecharge()
    {
        long erInMillis = (long)energyRechargeIntervalInSeconds * 1000;
        long ts = YG2.saves.nextEnergyRechargeTimeStamp;
        long cts = YG2.ServerTime();
        int energy = YG2.saves.energyLeft;
        if (energy >= maxEnergy)
        {
            YG2.saves.nextEnergyRechargeTimeStamp = 0;
            YG2.SaveProgress();
            UpdateEnergySlider(false, maxEnergy);
            playButton.interactable = true;
            return;
        }
        if (cts < ts)
        {
            if (energy == 0)
            {
                UpdateEnergySlider(true, (YG2.saves.nextEnergyRechargeTimeStamp - YG2.ServerTime()) / 1000);
                playButton.interactable = false;
            }
            else
            {
                UpdateEnergySlider(false, energy);
                playButton.interactable = true;
            }
            return;
        }
        while (cts >= ts - erInMillis)
        {
            energy++;
            ts += erInMillis;
        }
        if (energy >= maxEnergy)
        {
            YG2.saves.nextEnergyRechargeTimeStamp = 0;
            YG2.SaveProgress();
            UpdateEnergySlider(false, maxEnergy);
            playButton.interactable = true;
            return;
        }
        YG2.saves.nextEnergyRechargeTimeStamp += ts;
        YG2.saves.energyLeft = energy;
        YG2.SaveProgress();
        UpdateEnergySlider(false, energy);
    }

    private void Update()
    {
        int energy = YG2.saves.energyLeft;
        if (energy == maxEnergy) return;
        if (energy == 0)
        {
            UpdateEnergySlider(true, (YG2.saves.nextEnergyRechargeTimeStamp - YG2.ServerTime()) / 1000);
            return;
        }
        UpdateEnergySlider(false, energy);
    }

    private void UpdateEnergySlider(bool zero, decimal number)
    {
        if (!zero)
        {
            energySlider.value = (int)number;
            energyText.text = $"{(int)number}";
        }
        else
        {
            energySlider.value = 0;
            long m = (long)number / 60;
            long s = (long)number % 60;
            energyText.text = (m > 9 ? m : $"0{m}") + ":" + (s > 9 ? s : $"0{s}");
        }
    }

    public void PlayAD()
    {
        if (YG2.saves.energyLeft == maxEnergy) return;
        adPanel.SetActive(true);
        gameObject.SetActive(false);
        AudioListener.volume = 0;
        YG2.RewardedAdvShow("1", () => AddEnergyByAd());
    }


    public void AddEnergyByAd()
    {
        adPanel.SetActive(false);
        gameObject.SetActive(true);
        YG2.saves.energyLeft = Mathf.Clamp(YG2.saves.energyLeft + energyGainPerAd, 1, maxEnergy);
        YG2.SaveProgress();
        UpdateEnergySlider(false, YG2.saves.energyLeft);
        playButton.interactable = true;
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
    }

    internal void Spend()
    {
        if (YG2.saves.energyLeft == maxEnergy)
        {
            YG2.saves.nextEnergyRechargeTimeStamp = YG2.ServerTime() + energyRechargeIntervalInSeconds * 1000;
        }
        YG2.saves.energyLeft--;
        YG2.SaveProgress();
    }
}
