using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    public int money = 6000;
    public int maxMoney = 99999;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;          
            FindObjectOfType<UIManager>().UpdateMoneyUI();
            return true;
        }

        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        money = Mathf.Clamp(money, 0, maxMoney);
        FindObjectOfType<UIManager>().UpdateMoneyUI();
    }

    public int GetMoney()
    {
        return money;
    }
}
