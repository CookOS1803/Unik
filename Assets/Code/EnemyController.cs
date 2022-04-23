using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    private NavMeshAgent agent;
    private int currentPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.4f)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);

            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }        
    }
}