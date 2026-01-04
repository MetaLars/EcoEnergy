using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAI : MonoBehaviour
{
    private Vector2Int currentPos;
    private Vector2Int targetPos;

    private Vector2Int previousPos;

    private Dictionary<Vector2Int, List<Vector2Int>> roadGraph;

    private float speed = 2f;

    public void SetStartPosition(Vector2Int startPos, Dictionary<Vector2Int, List<Vector2Int>> graph)
    {
        previousPos = startPos;
        currentPos = startPos;
        roadGraph = graph;
        PickNewTarget();
    }

    void PickNewTarget()
    {
        if (roadGraph == null || !roadGraph.ContainsKey(currentPos))
            return;

        List<Vector2Int> neighbors = roadGraph[currentPos];

        if (neighbors.Count == 0)
            return;

        // Geldiði yönü çýkar
        List<Vector2Int> filteredNeighbors = new List<Vector2Int>();
        foreach (var n in neighbors)
        {
            if (n != previousPos)
                filteredNeighbors.Add(n);
        }

        // Eðer tüm komþular sadece geldiði yönse, onu da kabul et (kilitlenmesin)
        if (filteredNeighbors.Count == 0)
            filteredNeighbors = neighbors;

        targetPos = filteredNeighbors[Random.Range(0, filteredNeighbors.Count)];

        StopAllCoroutines();
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        Vector3 targetWorld = GridToWorld(targetPos);
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, targetWorld) > 0.05f)
        {
            // Hareket yönü
            Vector3 direction = targetWorld - transform.position;
            direction.y = 0f;

            // Smooth yön dönüþü
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Smooth pozisyon hareketi
            transform.position = Vector3.SmoothDamp(transform.position, targetWorld, ref velocity, 0.1f, speed);

            yield return null;
        }

        // Varýþ
        previousPos = currentPos;
        currentPos = targetPos;
        PickNewTarget();
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        // Yol prefab'ýnýn ortasýna gitmek için offset uygula
        return new Vector3(gridPos.x + 0f, 0.1f, gridPos.y + 0f);
    }
}
