using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    public int requiredLevel = 0;
    public GameObject lockIcon;

    private Button button;

    public int buildingIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void TrySelectBuilding()
    {
        if (LevelManager.Instance.currentLevel >= requiredLevel)
        {
            GridManager.Instance.SelectBuildingByIndex(buildingIndex);
        }
        else
        {
            Debug.Log("Kart kilitli, seçim yapýlamaz.");
        }
    }
    public void UpdateLock(int currentLevel)
    {
        bool isUnlocked = currentLevel >= requiredLevel;

        if (lockIcon != null)
            lockIcon.SetActive(!isUnlocked);

        if (button != null)
            button.interactable = isUnlocked;
    }
}
