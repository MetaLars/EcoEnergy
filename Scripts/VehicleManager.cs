using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager Instance;

    [Header("Araç Ayarlarý")]
    public GameObject[] vehiclePrefabs;

    private List<GameObject> activeVehicles = new List<GameObject>();
    private List<Vector2Int> roadPositions = new List<Vector2Int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TrySpawnVehicleIfNeeded()
    {
        UpdateRoadPositions();

        int requiredVehicleCount = roadPositions.Count / 20;
        int currentVehicleCount = activeVehicles.Count;

        if (requiredVehicleCount > currentVehicleCount)
        {
            int newVehiclesToSpawn = requiredVehicleCount - currentVehicleCount;

            for (int i = 0; i < newVehiclesToSpawn; i++)
            {
                SpawnSingleVehicle();
            }
        }
    }

    public void RebuildVehicles()
    {
        ClearVehicles();
        UpdateRoadPositions();

        int vehicleCount = roadPositions.Count / 20;

        for (int i = 0; i < vehicleCount; i++)
        {
            SpawnSingleVehicle();
        }
    }

    void UpdateRoadPositions()
    {
        roadPositions.Clear();

        if (RoadNetworkManager.Instance != null)
        {
            roadPositions = RoadNetworkManager.Instance.GetAllRoadPositions();
        }
    }

    void SpawnSingleVehicle()
    {
        if (roadPositions.Count == 0 || vehiclePrefabs.Length == 0)
            return;

        Vector2Int spawnPos = roadPositions[Random.Range(0, roadPositions.Count)];
        Vector3 worldPos = new Vector3(spawnPos.x + 0.5f, 0.1f, spawnPos.y + 0.5f);

        // Hedef yönü al - böylece doðru rotasyon verilebilir
        Vector2Int target = spawnPos;
        Dictionary<Vector2Int, List<Vector2Int>> graph = RoadNetworkManager.Instance.roadGraph;

        if (graph.ContainsKey(spawnPos) && graph[spawnPos].Count > 0)
        {
            target = graph[spawnPos][Random.Range(0, graph[spawnPos].Count)];
        }

        Vector3 targetWorldPos = new Vector3(target.x + 0.5f, 0.1f, target.y + 0.5f);
        Vector3 dir = (targetWorldPos - worldPos).normalized;
        Quaternion lookRot = dir != Vector3.zero ? Quaternion.LookRotation(dir) : Quaternion.identity;

        GameObject prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
        GameObject vehicle = Instantiate(prefab, worldPos, lookRot);
        activeVehicles.Add(vehicle);

        VehicleAI ai = vehicle.GetComponent<VehicleAI>();
        if (ai != null)
        {
            ai.SetStartPosition(spawnPos, graph);
        }
    }

    public int GetRoadCount()
    {
        UpdateRoadPositions();
        return roadPositions.Count;
    }

    void ClearVehicles()
    {
        foreach (var v in activeVehicles)
        {
            if (v != null)
                Destroy(v);
        }
        activeVehicles.Clear();
    }
}
