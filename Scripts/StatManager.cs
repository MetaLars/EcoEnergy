using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    public int totalEnergy = 0;
    public int totalNature = 0;
    public int totalWater = 0;
    public int totalSecurity = 0;
    public int totalHappiness = 0;
    public int totalPopulation = 0;

    private float statCheckTimer = 0f;
    private float checkInterval = 45f; // 30 saniyede bir kontrol

    private bool securityBonus = false;

    void Update()
    {
        statCheckTimer += Time.deltaTime;

        if (statCheckTimer >= checkInterval)
        {
            statCheckTimer = 0f;

            if (totalEnergy < 0 || totalWater < 0 || totalNature < 0 || totalSecurity < 0)
            {
                int lostPopulation = Mathf.CeilToInt(totalPopulation * 0.05f);
                totalPopulation = Mathf.Max(0, totalPopulation - lostPopulation);

                totalHappiness = Mathf.Max(0, totalHappiness - 2);

                UIManager.Instance.RefreshStatUI();
            }
        }

        if (!securityBonus && totalSecurity >= 20)
        {
            totalHappiness += 10;
            securityBonus = true;
            UIManager.Instance.RefreshStatUI();
        }
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddStats(Building b)
    {
        totalEnergy += b.energy;
        totalNature += b.nature;
        totalWater += b.water;
        totalSecurity += b.security;
        totalHappiness += b.happiness;
        totalPopulation += b.population;
    }

    public void RemoveStats(Building b)
    {
        totalEnergy -= b.energy;
        totalNature -= b.nature;
        totalWater -= b.water;
        totalSecurity -= b.security;
        totalHappiness -= b.happiness;
        totalPopulation -= b.population;
    }
}
