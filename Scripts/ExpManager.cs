using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI levelText;

    private void Update()
    {
        UpdateLevelBar();
    }

    void UpdateLevelBar()
    {
        int currentLevel = LevelManager.Instance.GetCurrentLevel();
        int currentPopulation = StatManager.Instance.totalPopulation;

        int[] thresholds = LevelManager.Instance.populationThresholds;

        // Þu anki seviyenin popülasyon aralýðýný belirle
        int currentLevelMin = thresholds[currentLevel - 1];
        int currentLevelMax = currentLevel < thresholds.Length ? thresholds[currentLevel] : currentLevelMin + 50; // Son seviyede biraz sabit deðer

        // Popülasyonun seviyedeki ilerlemesi
        float fillAmount = Mathf.InverseLerp(currentLevelMin, currentLevelMax, currentPopulation);

        fillImage.fillAmount = fillAmount;
        levelText.text = "" + currentLevel;
    }
}
