using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SuggestionSystem : MonoBehaviour
{
    public static SuggestionSystem Instance;

    [Header("Animasyon Panel Objeleri")]
    public GameObject resourceImbalancePanel;
    public GameObject happinessDropPanel;
    public GameObject populationStagnantPanel;
    public GameObject populationDecreasePanel;
    public GameObject noBuildingActivityPanel;
    public GameObject tooManySameBuildingPanel;
    public GameObject happinessStuckPanel;
    public GameObject resourceNegativePanel;

    [Header("Kontrol Süresi")]
    public float checkInterval = 20f;

    private Queue<GameObject> suggestionQueue = new Queue<GameObject>();
    private bool isDisplayingSuggestion = false;

    private float previousHappiness;
    private float sameHappinessTimer = 0f;
    private int previousPopulation;
    private float lastPopulationIncreaseTime;
    private float lastBuildTime;

    private StatManager statManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        statManager = StatManager.Instance;

        previousHappiness = statManager.totalHappiness;
        previousPopulation = statManager.totalPopulation;
        lastPopulationIncreaseTime = Time.time;
        lastBuildTime = Time.time;

        StartCoroutine(SuggestionChecker());
    }

    private void Update()
    {
        float currentHappiness = statManager.totalHappiness;

        if (Mathf.Approximately(currentHappiness, previousHappiness))
            sameHappinessTimer += Time.deltaTime;
        else
        {
            sameHappinessTimer = 0f;
            previousHappiness = currentHappiness;
        }
    }

    private IEnumerator SuggestionChecker()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            float energy = statManager.totalEnergy;
            float water = statManager.totalWater;
            float nature = statManager.totalNature;
            float happiness = statManager.totalHappiness;
            int currentPopulation = statManager.totalPopulation;

            // 1. Kaynak dengesizliði  (Kaynaklar dengesiz, dengeyi korumaya çalýþ )
            if (Mathf.Abs(energy - water) > 20 || Mathf.Abs(water - nature) > 20 || Mathf.Abs(energy - nature) > 20)
                EnqueueSuggestion(resourceImbalancePanel);

            // 2. Mutluluk düþüþü (Mutluluðun düþüyor, kaynaklarýný kontrol et)
            if (happiness < previousHappiness)
                EnqueueSuggestion(happinessDropPanel);

            // 3. Popülasyon duraklamasý (Popülasyon uzun süredir durgun þehrinin büyütmeyi dene)
            if (currentPopulation > previousPopulation)
                lastPopulationIncreaseTime = Time.time;
            else if (Time.time - lastPopulationIncreaseTime > 120f)
                EnqueueSuggestion(populationStagnantPanel);

            // 4. Popülasyon düþüþü (Þehrindeki insanlar ayrýlýyor, istihdamý saðlamalýsýn)
            if (currentPopulation < previousPopulation)
                EnqueueSuggestion(populationDecreasePanel);

            // 5. Uzun süredir bina yerleþtirilmemiþse (Þehrin durgun görünüyor)
            if (Time.time - lastBuildTime > 120f)
                EnqueueSuggestion(noBuildingActivityPanel);

            // 6. Ayný tip bina çok fazlaysa (örnek olarak "Factory")
            int factoryCount = CountBuildingsOfType(BuildingType.Factory);
            if (factoryCount >= 10)
                EnqueueSuggestion(tooManySameBuildingPanel);

            // 7. Mutluluk uzun süre sabit kaldýysa
            if (sameHappinessTimer > 100f)
                EnqueueSuggestion(happinessStuckPanel);

            // 8. Herhangi bir kaynak -20'nin altýndaysa (Kaynaklarýn fazla yetersiz, oyunu kaybetmeden önce kaynak inþa et)
            if (energy < -15 || water < -15 || nature < -15)
                EnqueueSuggestion(resourceNegativePanel);

            previousHappiness = happiness;
            previousPopulation = currentPopulation;
        }
    }

    private void EnqueueSuggestion(GameObject panel)
    {
        if (!suggestionQueue.Contains(panel))
        {
            suggestionQueue.Enqueue(panel);
            if (!isDisplayingSuggestion)
                StartCoroutine(ShowSuggestions());
        }
    }

    private IEnumerator ShowSuggestions()
    {
        isDisplayingSuggestion = true;

        while (suggestionQueue.Count > 0)
        {
            GameObject currentPanel = suggestionQueue.Dequeue();
            currentPanel.SetActive(true);

            yield return new WaitForSeconds(2f); // Animasyon süresi

            currentPanel.SetActive(false);
        }

        isDisplayingSuggestion = false;
    }

    // Bu fonksiyon GridManager'dan bina yerleþtirildiðinde çaðrýlmalý
    public void NotifyBuildingPlaced()
    {
        lastBuildTime = Time.time;
    }

    private int CountBuildingsOfType(BuildingType type)
    {
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("building");
        int count = 0;

        foreach (GameObject obj in allBuildings)
        {
            Building building = obj.GetComponent<Building>();
            if (building != null && building.buildingType == type)
                count++;
        }

        return count;
    }
}