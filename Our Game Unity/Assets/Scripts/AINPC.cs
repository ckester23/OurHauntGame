using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINPC : MonoBehaviour
{
    // The AI's current state
    private enum AIState
    {
        Wander,
        Flee,
    }

    private AIState currentState = AIState.Wander;
    private Transform player;
    private float wanderRadius = 10f;
    private float fleeDistance = 5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("UpdateAI", 1f, 1f); // Update AI every 1 second
    }

    void UpdateAI()
    {
        switch (currentState)
        {
            case AIState.Wander:
                Wander();
                break;

            case AIState.Flee:
                Flee();
                break;
        }
    }

    void Wander()
    {
        // Generate a random point within the wanderRadius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        Vector3 targetPosition = navHit.position;

        // Move towards the target position
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(targetPosition);

        // Check if the player is within fleeDistance
        if (Vector3.Distance(transform.position, player.position) < fleeDistance)
        {
            currentState = AIState.Flee;
        }
    }

    void Flee()
    {
        // Calculate a direction away from the player
        Vector3 fleeDirection = transform.position - player.position;
        fleeDirection.Normalize();

        // Calculate a target position to flee to
        Vector3 targetPosition = transform.position + fleeDirection * fleeDistance;

        // Move towards the target position
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(targetPosition);

        // Check if the player is no longer within fleeDistance to return to wandering
        if (Vector3.Distance(transform.position, player.position) >= fleeDistance)
        {
            currentState = AIState.Wander;
        }
    }
}

