using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxSystem : MonoBehaviour
{
    public float taxInterval = 10f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= taxInterval)
        {
            CollectTaxes();
            timer = 0f;
        }
    }

    private void CollectTaxes()
    {
        int totalIncome = 0;

        foreach (Building building in FindObjectsOfType<Building>())
        {
            if (building.buildingType == BuildingType.House)
            {
                totalIncome += building.taxIncome;
            }
        }

        // Mutluluk bonus çarpaný
        float happiness = StatManager.Instance.totalHappiness;
        float bonusMultiplier = 1f;

        if (happiness >= 50)
            bonusMultiplier = 1.3f;
        else if (happiness >= 30)
            bonusMultiplier = 1.2f;
        else if (happiness >= 15)
            bonusMultiplier = 1.1f;

        int finalIncome = Mathf.RoundToInt(totalIncome * bonusMultiplier);

        EconomyManager.Instance.AddMoney(finalIncome);
    }
}
