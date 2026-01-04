using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject[] buildingPrefabs;
    public int width = 50;
    public int height = 50;

    private TileRe[,] tiles;
    private GameObject selectedBuildingPrefab;

    public GameObject[] ghostPrefabs;
    private GameObject currentGhostInstance;

    public static GridManager Instance;
    public bool isDeleteMode = false;

    private bool wasInDeleteMode = false;

    private int currentRotation = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        tiles = new TileRe[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x, 0, z);
                GameObject tileObj = Instantiate(tilePrefab, pos, Quaternion.identity);
                TileRe tile = tileObj.GetComponent<TileRe>();
                tile.gridPosition = new Vector2Int(x, z);
                tiles[x, z] = tile;
            }
        }

        selectedBuildingPrefab = null;
    }

    void Update()
    {
        if (isDeleteMode)
        {
            if (currentGhostInstance != null && currentGhostInstance.activeSelf)
            {
                currentGhostInstance.SetActive(false);
            }

            wasInDeleteMode = true;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    Building building = clickedObject.GetComponentInParent<Building>();
                    if (building != null)
                    {
                        RemoveBuilding(building.gameObject);
                    }
                }
            }

            return;
        }
        else if (wasInDeleteMode)
        {
            if (currentGhostInstance != null)
            {
                currentGhostInstance.SetActive(true);
            }

            wasInDeleteMode = false;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit h))
        {
            TileRe hoveredTile = h.collider.GetComponent<TileRe>();

            if (hoveredTile != null && currentGhostInstance != null && selectedBuildingPrefab != null)
            {
                Vector2Int pos = hoveredTile.gridPosition;

                Building buildingInfo = selectedBuildingPrefab.GetComponent<Building>();
                GetRotatedSize(buildingInfo, out int w, out int hSize);

                Vector3 placePos = new Vector3(pos.x + w / 2f - 0.5f, 0, pos.y + hSize / 2f - 0.5f);
                currentGhostInstance.transform.position = placePos;
                currentGhostInstance.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
                currentGhostInstance.SetActive(true);

                if (CanPlace(pos.x, pos.y, w, hSize))
                    SetGhostColor(currentGhostInstance, Color.green);
                else
                    SetGhostColor(currentGhostInstance, Color.red);

                // Sol tıkla bina yerleştir
                if (Input.GetMouseButtonDown(0))
                {
                    hoveredTile.OnMouseDown();
                }
            }
            else
            {
                // ❌ Mouse bir tile üzerinde değilse
                if (currentGhostInstance != null)
                {
                    currentGhostInstance.SetActive(false);
                }
            }
        }
        else
        {
            // ❌ Raycast hiçbir şeye çarpmadıysa
            if (currentGhostInstance != null)
            {
                currentGhostInstance.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation = (currentRotation + 90) % 360;
            if (currentGhostInstance != null)
            {
                currentGhostInstance.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
            }
        }
    }

    void SetGhostColor(GameObject ghost, Color color)
    {
        Renderer[] renderers = ghost.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            foreach (Material mat in r.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color transparentColor = color;
                    transparentColor.a = 0.4f; // Şeffaflık ayarı
                    mat.color = transparentColor;

                    // Eğer material cutout veya opaque ise, bunu transparan yap:
                    if (mat.HasProperty("_Surface")) // URP için
                    {
                        mat.SetFloat("_Surface", 1); // Transparent
                    }
                }              
            }
        }
    }

    public void GetRotatedSize(Building building, out int rotatedWidth, out int rotatedHeight)
    {
        if (currentRotation % 180 == 0)
        {
            rotatedWidth = building.width;
            rotatedHeight = building.height;
        }
        else
        {
            rotatedWidth = building.height;
            rotatedHeight = building.width;
        }
    }

    public TileRe GetTileAtPosition(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
        {
            return tiles[pos.x, pos.y];
        }
        return null;
    }

    public GameObject GetSelectedBuildingPrefab()
    {
        return selectedBuildingPrefab;
    }

    public void SelectBuildingByIndex(int index)
    {
        if (index >= 0 && index < buildingPrefabs.Length)
        {
            selectedBuildingPrefab = buildingPrefabs[index];

            // Mevcut ghost'u sil
            if (currentGhostInstance != null)
                Destroy(currentGhostInstance);

            // Yeni ghost'u oluştur
            currentGhostInstance = Instantiate(ghostPrefabs[index]);
            currentGhostInstance.SetActive(false);
        }
    }

    public void SpawnBuildingAt(Vector2Int origin)
    {
        Building buildingInfo = selectedBuildingPrefab.GetComponent<Building>();
        GetRotatedSize(buildingInfo, out int w, out int h);

        if (!CanPlace(origin.x, origin.y, w, h))
        {
            Debug.Log("Bu alana bina yerleştirilemez!");
            return;
        }

        Vector3 placePos = new Vector3(origin.x + w / 2f - 0.5f, 0, origin.y + h / 2f - 0.5f);
        GameObject newBuilding = Instantiate(selectedBuildingPrefab, placePos, Quaternion.Euler(0, currentRotation, 0));
        newBuilding.transform.SetParent(transform);
  

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
            {
                int tileX = origin.x + x;
                int tileZ = origin.y + z;

                if (tileX >= 0 && tileX < width && tileZ >= 0 && tileZ < height)
                {
                    tiles[tileX, tileZ].SetCurrentBuilding(newBuilding); // Burada currentBuilding setleniyor
                }
            }
        }

        if (buildingInfo.buildingType == BuildingType.Road)
        {
            RoadNetworkManager.Instance.RebuildRoadNetwork();

            // ❗ Sadece 20'den fazla yol varsa araçları yeniden oluştur
            int roadCount = VehicleManager.Instance.GetRoadCount();
            if (roadCount >= 20)
            {
                VehicleManager.Instance.TrySpawnVehicleIfNeeded();
            }
        }


        SuggestionSystem.Instance.NotifyBuildingPlaced();
        UIManager.Instance.RefreshStatUI();
    }

    public void RemoveBuilding(GameObject building)
    {
        if (building == null) return;

        Building buildingInfo = building.GetComponent<Building>();
        if (buildingInfo != null && buildingInfo.buildingType != BuildingType.Road)
        {
            StatManager.Instance.RemoveStats(buildingInfo);
            int refundAmount = Mathf.RoundToInt(buildingInfo.cost * 0.2f);
            EconomyManager.Instance.AddMoney(refundAmount);
        }

        // Bina ile ilişkili tüm tile'ları temizle
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                TileRe tile = tiles[x, z];
                if (tile != null && tile.GetCurrentBuilding() == building)
                {
                    tile.SetCurrentBuilding(null); // null atayıp kullanılabilir yap
                    tile.hasBuilding = false;
                }
            }
        }
        Destroy(building);
        UIManager.Instance.RefreshStatUI();
    }

    bool IsAdjacentToRoad(int x, int z)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
        };

        foreach (var dir in directions)
        {
            int checkX = x + dir.x;
            int checkZ = z + dir.y;

            // Grid sınır kontrolü
            if (checkX >= 0 && checkX < width && checkZ >= 0 && checkZ < height)
            {
                TileRe neighbor = tiles[checkX, checkZ];

                if (neighbor != null && neighbor.hasBuilding)
                {
                    GameObject neighborObj = neighbor.GetCurrentBuilding();
                    if (neighborObj != null)
                    {
                        Building neighborBuilding = neighborObj.GetComponent<Building>();
                        if (neighborBuilding != null && neighborBuilding.buildingType == BuildingType.Road)
                        {
                            return true; // En az bir komşu yolsa yeter
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool CanPlace(int x, int z, int w, int h)
    {
        if (x < 0 || z < 0 || x + w > width || z + h > height)
            return false;

        bool isNextToRoad = false;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                TileRe tile = tiles[x + i, z + j];
                if (tile == null || tile.hasBuilding)
                    return false;

                if (!isNextToRoad && IsAdjacentToRoad(x + i, z + j))
                {
                    isNextToRoad = true;
                }
            }
        }
        // Eğer bina yol gerektiriyorsa ve hiçbir kenarı yola değmiyorsa -> izin verme
        if (selectedBuildingPrefab != null)
        {
            Building building = selectedBuildingPrefab.GetComponent<Building>();
            if (building != null && building.requiresRoad && !isNextToRoad)
            {               
                return false;
            }
        }

        return true;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
    }

}
