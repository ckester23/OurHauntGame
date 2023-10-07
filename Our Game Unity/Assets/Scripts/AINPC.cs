using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothWanderAI : MonoBehaviour
{
    public float wanderSpeed = 2f;
    public float fleeSpeed = 4f;
    public float fleeDistance = 5f;
    public float visionDistance = 10f;
    public LayerMask itemLayer;
    public LayerMask obstacleLayer;

    private Transform player;
    private bool isFleeing = false;
    private Vector3 wanderTarget;
    private Vector3 wanderDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Start wandering immediately
        SetRandomWanderTarget();

        visionCollider = GetComponent<Collider>();
    }

    private List<GameObject> carriedItems = new List<GameObject>();

    // Reference to the NPC's vision collider (assumed to be a trigger collider)
    private Collider visionCollider;



    private void OnTriggerEnter(Collider other)
    {
        // Check if the NPC has collided with an item
        if (other.CompareTag("Item"))
        {
            // Get the ItemScript component from the collided item
            ItemScript itemScript = other.GetComponent<ItemScript>();

            // Check if the item is active (not collected) and the NPC has room to carry it
            if (itemScript != null && itemScript.gameObject.activeSelf && carriedItems.Count < 3)
            {
                // Collect the item and add it to the NPC's carried items
                itemScript.Collect();
                carriedItems.Add(other.gameObject);
            }
        }
    }

    private void Update()
    {
        if (player == null)
            return; // Player not found, do nothing

        // Calculate the distance between the AI and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isFleeing)
        {
            // Flee from the player
            Vector3 fleeDirection = transform.position - player.position;
            fleeDirection.y = 0f; // Ensure the AI moves only in the X-Z plane
            transform.position += fleeDirection.normalized * fleeSpeed * Time.deltaTime;

            // If the player is no longer within flee distance, stop fleeing
            if (distanceToPlayer > fleeDistance)
                isFleeing = false;
        }
        else
        {
            // Wander behavior
            if (distanceToPlayer < visionDistance && IsPlayerVisible())
            {
                // Player is in sight, start fleeing
                isFleeing = true;
            }
            else
            {
                // Smooth wandering with obstacle avoidance
                Vector3 desiredPosition = transform.position + wanderDirection * wanderSpeed * Time.deltaTime;
                Vector3 avoidanceDirection = AvoidObstacles(desiredPosition);

                // Adjust the wander direction based on obstacle avoidance
                wanderDirection = avoidanceDirection.normalized;

                // Move towards the wander target
                transform.position += wanderDirection * wanderSpeed * Time.deltaTime;

                // Set a new wander target if needed
                if (Vector3.Distance(transform.position, wanderTarget) < 1f)
                {
                    SetRandomWanderTarget();
                }

                // Collect items if they are within reach
                CollectItems();
            }
        }
    }

    private void SetRandomWanderTarget()
    {
        // Set a random point within a certain range as the wander target
        float randomX = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);
        wanderTarget = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Calculate a new wander direction
        wanderDirection = (wanderTarget - transform.position).normalized;
    }

    private void CollectItems()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, itemLayer);
        foreach (var collider in colliders)
        {
            // Assuming you have a script on your items that handles the collection
            ItemScript itemScript = collider.GetComponent<ItemScript>();
            if (itemScript != null)
            {
                itemScript.Collect();
            }
        }
    }

    private bool IsPlayerVisible()
    {
        // Check if the player is visible (alpha is not zero)
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            return playerRenderer.material.color.a > 0f;
        }
        return true; // If the player doesn't have a renderer or material, assume it's visible
    }

    private Vector3 AvoidObstacles(Vector3 desiredPosition)
    {
        // Perform obstacle avoidance here based on the obstacleLayer
        RaycastHit hit;
        if (Physics.Raycast(transform.position, wanderDirection, out hit, 2f, obstacleLayer))
        {
            // If an obstacle is detected, adjust the desired position
            desiredPosition = hit.point - wanderDirection * 2f;
        }
        return desiredPosition;
    }
}



