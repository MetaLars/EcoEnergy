using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetworkManager : MonoBehaviour
{
    public static RoadNetworkManager Instance;

    private HashSet<Vector2Int> roadPositions = new HashSet<Vector2Int>();

    // Yeni eklenecek: Yol aðýný tutacak sözlük (komþuluk listesi)
    public Dictionary<Vector2Int, List<Vector2Int>> roadGraph = new Dictionary<Vector2Int, List<Vector2Int>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RebuildRoadNetwork()
    {
        roadPositions.Clear();

        for (int x = 0; x < GridManager.Instance.width; x++)
        {
            for (int y = 0; y < GridManager.Instance.height; y++)
            {
                TileRe tile = GridManager.Instance.GetTileAtPosition(new Vector2Int(x, y));
                if (tile != null && tile.hasBuilding)
                {
                    GameObject building = tile.GetCurrentBuilding();
                    if (building != null)
                    {
                        Building b = building.GetComponent<Building>();
                        if (b != null && b.buildingType == BuildingType.Road)
                        {
                            roadPositions.Add(tile.gridPosition);
                        }
                    }
                }
            }
        }

        BuildRoadGraph();
    }

    // Yol komþuluk listesini oluþturuyoruz
    private void BuildRoadGraph()
    {
        roadGraph.Clear();

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };

        foreach (var pos in roadPositions)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            foreach (var dir in directions)
            {
                Vector2Int neighborPos = pos + dir;
                if (roadPositions.Contains(neighborPos))
                {
                    neighbors.Add(neighborPos);
                }
            }

            roadGraph[pos] = neighbors;
        }
    }

    public List<Vector2Int> GetAllRoadPositions()
    {
        return new List<Vector2Int>(roadPositions);
    }

    public Vector2Int GetRandomRoadPosition()
    {
        List<Vector2Int> roads = GetAllRoadPositions();
        if (roads.Count == 0) return Vector2Int.zero;
        return roads[Random.Range(0, roads.Count)];
    }

    public int GetTotalRoadCount()
    {
        return roadPositions.Count;
    }

}
