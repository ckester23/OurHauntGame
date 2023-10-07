using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAI : MonoBehaviour
{
    public float wanderSpeed = 2f;
    public float fleeSpeed = 4f;
    public float fleeDistance = 20f;
    public float visionDistance = 10f;

    private Transform player;
    private bool isFleeing = false;
    private Vector3 wanderTarget;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Start wandering immediately
        SetRandomWanderTarget();
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
            if (distanceToPlayer < visionDistance)
            {
                // Player is in sight, start fleeing
                isFleeing = true;
            }
            else if (Vector3.Distance(transform.position, wanderTarget) < 1f)
            {
                // Reached the current wander target, set a new one
                SetRandomWanderTarget();
            }

            // Move towards the wander target
            Vector3 moveDirection = wanderTarget - transform.position;
            moveDirection.y = 0f; // Ensure the AI moves only in the X-Z plane
            transform.position += moveDirection.normalized * wanderSpeed * Time.deltaTime;
        }
    }

    private void SetRandomWanderTarget()
    {
        // Set a random point within a certain range as the wander target
        float randomX = Random.Range(-20f, 20f);
        float randomZ = Random.Range(-20f, 20f);
        wanderTarget = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }
}


