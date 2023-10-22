using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// Original Patrolling script from: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
public class NewNPCScript : MonoBehaviour
{
    public float wanderSpeed = 2f;
    public float fleeSpeed = 4f;

    public float fleeDistance = 3f;
    public float visionDistance = 10f;

    public int fearLevel = 100;

    public Transform waypoint;

    private Transform center;
    private Transform exit;
    private Transform player;
    private NavMeshAgent agent;

    private bool isFleeing = false;
    private Vector3 wanderTarget;


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

        agent.speed = wanderSpeed;

        agent.autoBraking = false;

        GenerateNewWaypoint();
    }

     void GenerateNewWaypoint()
    {
        float wayX;
        float wayZ;

        if (isFleeing)
        {
            // run towards exit
            // cap this, so they don't just leave every time they get spooked
            // if player does an ultimate scare, they do leave
            wanderTarget = new Vector3(exit.position.x, transform.position.y, exit.position.z);
            agent.speed = fleeSpeed;
        }
        else
        {
            // select a position within the whole map, add these to the center point of map
            wayX = Random.Range(-53f, 15f);
            wayZ = Random.Range(-13f, 50f);
            //wayX = Random.Range(-1f*range, range);
            //wayZ = Random.Range(-1f * range, range);
            wanderTarget = new Vector3(center.position.x + wayX, transform.position.y, center.position.z + wayZ);
            agent.speed = wanderSpeed;
        }
        waypoint.position = wanderTarget;
        agent.destination = waypoint.position;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GenerateNewWaypoint();
            if (isFleeing) isFleeing = !isFleeing;
        }

        // TODO Edit to happen when scared, not when player is too close
        // if player is too close, flee
        float distToPlayer = Vector3.Distance(player.position, transform.position);
        if (distToPlayer < fleeDistance)
        {
            isFleeing = true;
            GenerateNewWaypoint();
        }
    }
}
