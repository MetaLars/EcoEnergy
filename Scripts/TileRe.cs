using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRe : MonoBehaviour
{
    public Vector2Int gridPosition; 
    public bool hasBuilding = false;
    public GridManager gridManager;
    private Renderer tileRenderer;
    private Color originalColor;

    private GameObject currentBuilding;
    private void Start()
    {
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }

        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            originalColor = tileRenderer.material.color;
        }
    }

    public void OnMouseDown()
    {
        if (UIManager.Instance.IsPointerOverUI()) return;

        if (GridManager.Instance.isDeleteMode && hasBuilding && currentBuilding != null)
        {          
            GridManager.Instance.RemoveBuilding(currentBuilding);
            return;
        }

        if (!hasBuilding && !GridManager.Instance.isDeleteMode)
        {
            GameObject selectedBuilding = gridManager.GetSelectedBuildingPrefab();

            if (selectedBuilding != null)
            {
                Building building = selectedBuilding.GetComponent<Building>();
                int buildingCost = building.cost;

                gridManager.GetRotatedSize(building, out int w, out int h);
                if (!gridManager.CanPlace(gridPosition.x, gridPosition.y, w, h))
                {
                    UIManager.Instance.ShowWarning1();
                    return;
                }

                if (EconomyManager.Instance.SpendMoney(buildingCost))
                {
                    gridManager.SpawnBuildingAt(gridPosition);
                    hasBuilding = true;

                    StatManager.Instance.AddStats(building);

                    UIManager.Instance.RefreshStatUI();
                }
                else
                {
                    UIManager.Instance.ShowWarning();
                    return;
                }
            }
        }
    }

    public void SetCurrentBuilding(GameObject building)
    {
        currentBuilding = building;
        hasBuilding = true;
    }
    public GameObject GetCurrentBuilding()
    {
        return currentBuilding;
    }

    private void OnMouseEnter()
    {
        if (GridManager.Instance == null || tileRenderer == null)
            return;

        if (!hasBuilding && !GridManager.Instance.isDeleteMode)
        {
            tileRenderer.material.color = Color.green;
        }
        else if (hasBuilding && GridManager.Instance.isDeleteMode)
        {
            tileRenderer.material.color = Color.white;
        }      
    }

    private void OnMouseExit()
    {
        tileRenderer.material.color = originalColor;
    } 
}
