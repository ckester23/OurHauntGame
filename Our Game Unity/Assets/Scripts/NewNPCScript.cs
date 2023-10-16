using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// Original Patrolling script from: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
public class NewNPCScript : MonoBehaviour
{
    public float wanderSpeed = 2f;
    public float fleeSpeed = 4f;
    public float fleeDistance = 5f;
    public float visionDistance = 10f;
    /*public LayerMask itemLayer;
    public LayerMask obstacleLayer;
    */

    public Transform waypoint;

    private Transform center;
    private Transform exit;
    private Transform player;
    
    private bool isFleeing = false;
    private Vector3 wanderTarget;
    
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        // find myself and player
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        center = GameObject.FindGameObjectWithTag("CenterPoint").transform;
        exit = GameObject.FindGameObjectWithTag("Exit").transform;

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // if NPC is fleeing, don't reset the waypoint (shouldn't ever need to be in this section)
        if (isFleeing)
            return;

        // select a destination
        GenerateNewWaypoint(10f);

        // Set the agent to go to the destination.
        agent.destination = waypoint.position;

    }

    void Flee()
    {
        // select a point towards the exit, away from player
        GenerateNewWaypoint(0f);
    }

    void GenerateNewWaypoint(float range)
    {
        float wayX;
        float wayZ;

        if (isFleeing)
        {
            // run towards exit
            // cap this, so they don't just leave every time they get spooked
            // if player does an ultimate scare, they do leave
            wanderTarget = new Vector3(exit.position.x, transform.position.y, exit.position.z);

        }
        else
        {
            // select a position within the whole map, add these to the center point of map
            wayX = Random.Range(-53f, 15f);
            wayZ = Random.Range(-13f, 50f);
            //wayX = Random.Range(-1f*range, range);
            //wayZ = Random.Range(-1f * range, range);
            wanderTarget = new Vector3(center.position.x + wayX, transform.position.y, center.position.z + wayZ);

        }
        waypoint.position = wanderTarget;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            //Debug.Log("I'm getting close!");
            if (isFleeing) isFleeing = !isFleeing;
            GotoNextPoint();
        }

        // TODO Check where the player is
        // if too close, flee
            
    }
}
