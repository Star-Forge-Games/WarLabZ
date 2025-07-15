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
    [SerializeField] TextMeshProUGUI energyText, rechargeText;
    [SerializeField] int energyGainPerAd = 3;
    [SerializeField] GameObject adPanel;
    [SerializeField] private int maxEnergy = 15;
    [SerializeField] private long energyRechargeIntervalInSeconds;

    private void OnEnable()
    {
        energySlider.maxValue = maxEnergy;
        if (YG2.saves.playedBefore <= 0)
        {
            YG2.saves.energyLeft = maxEnergy;
            YG2.saves.nextEnergyRechargeTimeStamp = 0;
            YG2.SaveProgress();
            UpdateEnergySlider(maxEnergy);
            playButton.interactable = true;
            return;
        }
        if (YG2.saves.energyLeft >= maxEnergy)
        {
            YG2.saves.energyLeft = maxEnergy;
            YG2.saves.nextEnergyRechargeTimeStamp = 0;
            YG2.SaveProgress();
            UpdateEnergySlider(maxEnergy);
            playButton.interactable = true;
            return;
        }
        StartupRecharge();
    }

    private void StartupRecharge()
    {
        long erInMillis = energyRechargeIntervalInSeconds * 1000;
        long ts = YG2.saves.nextEnergyRechargeTimeStamp;
        long cts = Time();
        int energy = YG2.saves.energyLeft;
        if (energy >= maxEnergy)
        {
            YG2.saves.nextEnergyRechargeTimeStamp = 0;
            YG2.SaveProgress();
            UpdateEnergySlider(maxEnergy);
            playButton.interactable = true;
            return;
        }
        if (cts < ts)
        {
            if (energy == 0)
            {
                UpdateEnergySlider(0);
                playButton.interactable = false;
            }
            else
            {
                UpdateEnergySlider(energy);
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
            UpdateEnergySlider(maxEnergy);
            playButton.interactable = true;
            return;
        }
        YG2.saves.nextEnergyRechargeTimeStamp += ts;
        YG2.saves.energyLeft = energy;
        YG2.SaveProgress();
        UpdateEnergySlider(energy);
    }


    private long Time()
    {
#if UNITY_EDITOR
        return DateTime.Now.Ticks / 10000;
#else
    return YG2.ServerTime();
#endif
    }

    private void Update()
    {
        int energy = YG2.saves.energyLeft;
        if (energy >= maxEnergy) return;
        if (YG2.saves.nextEnergyRechargeTimeStamp <= Time())
        {
            energy++;
            YG2.saves.energyLeft = energy;
            YG2.saves.nextEnergyRechargeTimeStamp = energy == maxEnergy ? 0 : YG2.saves.nextEnergyRechargeTimeStamp + energyRechargeIntervalInSeconds * 1000;
            YG2.SaveProgress();
        }
        if (energy == 0)
        {
            UpdateEnergySlider(0);
            return;
        }
        UpdateEnergySlider(energy);
        playButton.interactable = true;
    }

    private void UpdateEnergySlider(int energy)
    {
        energySlider.value = energy;
        energyText.text = $"{energy}";
        if (energy == maxEnergy)
        {
            rechargeText.text = "";
            return;
        }
        long time = (YG2.saves.nextEnergyRechargeTimeStamp - Time()) / 1000;
        long m = time / 60;
        long s = time % 60;
        rechargeText.text = (m > 9 ? m : $"0{m}") + ":" + (s > 9 ? s : $"0{s}");
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
        UpdateEnergySlider(YG2.saves.energyLeft);
        playButton.interactable = true;
        AudioListener.volume = YG2.saves.soundOn ? 1 : 0;
    }

    internal void Spend()
    {
        if (YG2.saves.energyLeft == maxEnergy)
        {
            YG2.saves.nextEnergyRechargeTimeStamp = Time() + energyRechargeIntervalInSeconds * 1000;
        }
        YG2.saves.energyLeft--;
        YG2.SaveProgress();
    }
}
