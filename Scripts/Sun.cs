using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sun : MonoBehaviour
{
    [Range(0,1)] public float time;

    public float startTime;
    public float dayLenght;
    private float timeRate;
    public Vector3 noon;

    public int currentDay = 1;

    [Header("Gün Sayacı UI")]
    public TextMeshProUGUI dayText;

    public static Sun Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeRate = 1 / dayLenght;
        time = startTime;

        UpdateDayUI();
    }

    private void Update()
    {
        time += timeRate * Time.deltaTime;

        if (time >= 1f)
        {
            time = 0f;
            currentDay++;
            UpdateDayUI();
        }

        transform.eulerAngles = noon * ((time - 0.25f) * 4);
    }

    private void UpdateDayUI()
    {
        if (dayText != null)
        {
            dayText.text = "Gün: " + currentDay;
        }
    }
}
