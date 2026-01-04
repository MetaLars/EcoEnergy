using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentLevel = 1;

    [Header("Seviye eþikleri")]
    public int[] populationThresholds = { 0, 10, 25, 50, 100 }; // Seviye 1,2,3,4,5 için

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        CheckLevelByPopulation();
    }

    void CheckLevelByPopulation()
    {
        int population = StatManager.Instance.totalPopulation;

        for (int i = populationThresholds.Length - 1; i >= 0; i--)
        {
            if (population >= populationThresholds[i])
            {
                if (currentLevel != i + 1)
                {
                    currentLevel = i + 1;
                    UIManager.Instance.UpdateCardLocks(currentLevel); // Kartlar güncelleniyor
                }
                return;
            }
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
