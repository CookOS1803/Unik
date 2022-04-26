using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Normal, Alarmed
};

public class EnemyController : MonoBehaviour
{
    [SerializeField, Min(0f)] private float distanceOfView = 10f;
    [SerializeField, Range(0f, 360)] private float fieldOfView = 90f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField, Min(0f)] private float noticeTime = 2f;
    [SerializeField] private Transform[] patrolPoints;
    private NavMeshAgent agent;
    private int currentPoint;
    private float noticeClock = 0f;
    
    public Transform player { get; private set; }
    public EnemyState state { get; set; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        AIManager.enemies.Add(this);
    }

    void Update()
    {
        NoticePlayer();

        if (AIManager.player != null)
            agent.SetDestination(AIManager.player.transform.position);
    }

    private void NoticePlayer()
    {
        var cols = Physics.OverlapSphere(transform.position, distanceOfView, playerLayer.value);

        if (cols.Length != 0)
        {
            float angle = Vector3.Angle(transform.forward, cols[0].transform.position - transform.position);

            if (angle <= fieldOfView / 2f)
            {   
                RaycastHit hit;
                Physics.Linecast(transform.position + Vector3.up, cols[0].transform.position + Vector3.up, out hit);

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {

                    if (AIManager.alarm || noticeClock >= noticeTime)
                    {
                        player = hit.transform;
                        AIManager.SoundTheAlarm();
                    }
                    else
                    {                        
                        noticeClock += Time.deltaTime;
                    }
                    
                    return;
                }
            }
        }

        player = null;
        noticeClock = 0f;
    }
    
    public void FindPlayer()
    {
        StartCoroutine(FindingPlayer());
    }

    IEnumerator FindingPlayer()
    {
        float seekClock = 0f;

        while (AIManager.alarm && AIManager.player == null)
        {
            if (agent.velocity == Vector3.zero)
            {
                agent.isStopped = true;
                transform.Rotate(0f, Random.Range(-180f, 180f), 0f);
                
                while (AIManager.player == null && seekClock < 1f)
                {
                    seekClock += Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }

                seekClock = 0f;

                agent.isStopped = false;
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }

    void OnDestroy()
    {
        AIManager.enemies.Remove(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, distanceOfView);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, fieldOfView / 2f, 0f)  * (transform.forward) * distanceOfView);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, -fieldOfView / 2f, 0f) * (transform.forward) * distanceOfView);
    }
}