using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel; // UI panelin (örneğin bina seçim menüsü)
    public TextMeshProUGUI moneyText;
    private bool isUIOpen = true;

    public static UIManager Instance;

    public TextMeshProUGUI deleteModeText;

    public TMP_Text energyText;
    public TMP_Text natureText;
    public TMP_Text waterText;
    public TMP_Text securityText;
    public TMP_Text happinessText;
    public TMP_Text populationText;

    public Animator energyAnimator;
    public Animator natureAnimator;
    public Animator waterAnimator;
    public Animator securityAnimator;
    public Animator happinessAnimator;

    public CanvasGroup warningPanel;
    public CanvasGroup warning1Panel;
    public Animator warningAnimator;
    public Animator warning1Animator;

    public GameObject keyGuidePanel;
    private bool isKeyGuideOpen = false;

    public List<CardButton> allCardButtons;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateMoneyUI();
        OpenUI(); // Oyun başlarken UI açıksa
    }
    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = " " + EconomyManager.Instance.GetMoney().ToString();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isUIOpen = !isUIOpen;

            if (isUIOpen)
                OpenUI();
            else
                CloseUI();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleKeyGuide();
        }

    }

    public void UpdateCardLocks(int currentLevel)
    {
        foreach (CardButton card in allCardButtons)
        {
            card.UpdateLock(currentLevel);
        }
    }

    public void ToggleKeyGuide()
    {
        isKeyGuideOpen = !isKeyGuideOpen;
        keyGuidePanel.SetActive(isKeyGuideOpen);
    }

    public void ShowWarning()
    {
        warningAnimator.SetTrigger("Show");
    }
    public void ShowWarning1()
    {
        warning1Animator.SetTrigger("Show");
    }

    void OpenUI()
    {
        uiPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseUI()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnDeleteButtonClicked()
    {
        GridManager.Instance.ToggleDeleteMode();
        if (GridManager.Instance.isDeleteMode)
        {
            deleteModeText.text = "Açık";
            deleteModeText.color = Color.red;
        }
        else
        {
            deleteModeText.text = "Kapalı";
            deleteModeText.color = Color.white;
        }
    }

    public void RefreshStatUI()
    {
        energyText.text = " " + StatManager.Instance.totalEnergy;
        natureText.text = " " + StatManager.Instance.totalNature;
        waterText.text = " " + StatManager.Instance.totalWater;
        securityText.text = " " + StatManager.Instance.totalSecurity;
        happinessText.text = " " + StatManager.Instance.totalHappiness;
        populationText.text = " " + StatManager.Instance.totalPopulation;

        energyAnimator.SetTrigger("PlayAnim");
        natureAnimator.SetTrigger("PlayAnim");
        waterAnimator.SetTrigger("PlayAnim");
        securityAnimator.SetTrigger("PlayAnim");
        happinessAnimator.SetTrigger("PlayAnim");
    }
}
