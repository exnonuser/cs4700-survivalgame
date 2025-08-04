using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;    // Waypoints to patrol
    public float detectionRadius = 10f; // Distance to detect player
    public float chaseRadius = 15f;     // Distance to stop chasing player
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 4f;

    private int currentPatrolIndex;
    private Transform player;
    private NavMeshAgent agent;
    private bool isChasing;

    private Animator animator;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentPatrolIndex = 0;
        isChasing = false;
        animator.SetBool("isChasing", false);
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
    {
        Debug.Log("Is chasing: " + isChasing);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer <= detectionRadius)
        {
            // Start chasing player
            isChasing = true;
            animator.SetBool("isChasing", true);
            agent.speed = chaseSpeed;
        }
        else if (isChasing && distanceToPlayer > chaseRadius)
        {
            // Stop chasing, resume patrol
            isChasing = false;
            animator.SetBool("isChasing", false);
            agent.speed = patrolSpeed;
            GoToNextPatrolPoint();
        }

        if (isChasing)
        {
            // Chase player
            agent.SetDestination(player.position);
        }
        else
        {
            // Patrol
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPatrolPoint();
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
}