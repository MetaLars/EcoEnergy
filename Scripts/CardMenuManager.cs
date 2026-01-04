using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenuManager : MonoBehaviour
{
    public GameObject[] buildingButtons; // Tüm kartlar burada (önceden Unity'den atanacak)
    public int cardsPerPage = 8;
    public Button leftButton;
    public Button rightButton;

    private int currentPage = 0;

    void Start()
    {
        UpdatePage();
        leftButton.onClick.AddListener(PreviousPage);
        rightButton.onClick.AddListener(NextPage);
    }

    void UpdatePage()
    {
        for (int i = 0; i < buildingButtons.Length; i++)
        {
            buildingButtons[i].SetActive(i >= currentPage * cardsPerPage && i < (currentPage + 1) * cardsPerPage);
        }

        leftButton.interactable = currentPage > 0;
        rightButton.interactable = (currentPage + 1) * cardsPerPage < buildingButtons.Length;
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    void NextPage()
    {
        if ((currentPage + 1) * cardsPerPage < buildingButtons.Length)
        {
            currentPage++;
            UpdatePage();
        }
    }
}
