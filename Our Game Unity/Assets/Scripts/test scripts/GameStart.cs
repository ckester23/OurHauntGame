using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTargetPlacer : MonoBehaviour
{
    public GameObject investigationTargetPrefab; // Prefab of the InvestigationTarget object
    public int maxNumberOfTargets = 3; // Maximum number of targets to place
    public float yOffsetAboveGround = 0.1f; // Y-offset above the ground to prevent clipping

    private bool targetsPlaced = false;

    private void Start()
    {
        if (!targetsPlaced)
        {
            Debug.Log("Placing InvestigationTargets...");
            PlaceInvestigationTargets();
            targetsPlaced = true;
        }
    }

    private void PlaceInvestigationTargets()
    {
        // Check if the maximum number of targets has already been placed
        int existingTargetsCount = GameObject.FindGameObjectsWithTag("InvestigationTarget").Length;
        int targetsToPlace = Mathf.Max(0, maxNumberOfTargets - existingTargetsCount);

        for (int i = 0; i < targetsToPlace; i++)
        {
            Vector3 randomPosition = GenerateRandomNavMeshPosition();
            randomPosition.y += yOffsetAboveGround; // Offset above ground
            Instantiate(investigationTargetPrefab, randomPosition, Quaternion.identity);
        }

        Debug.Log("InvestigationTargets placed.");
    }

    private Vector3 GenerateRandomNavMeshPosition()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int randomTriangleIndex = Random.Range(0, navMeshData.indices.Length / 3);
        Vector3 v1 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3]];
        Vector3 v2 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3 + 1]];
        Vector3 v3 = navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3 + 2]];

        // Generate a random point within the selected triangle
        float r1 = Random.Range(0f, 1f);
        float r2 = Random.Range(0f, 1f);
        if (r1 + r2 > 1)
        {
            r1 = 1 - r1;
            r2 = 1 - r2;
        }

        return v1 + r1 * (v2 - v1) + r2 * (v3 - v1);
    }
}




