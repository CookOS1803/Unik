using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IMoveable
{
    [SerializeField, Min(0f)] private float distanceOfView = 10f;
    [SerializeField, Min(0f)] private float attackRange = 1f;
    [SerializeField, Range(0f, 360f)] private float fieldOfView = 90f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField, Min(0f)] private float noticeTime = 2f;
    [SerializeField, Min(0f)] private float findingRadius = 6f;
    [SerializeField, Min(0f)] private float waitTime = 2f;
    [SerializeField, Min(0f)] private float forgetTime = 0.5f;
    [SerializeField] private Transform[] patrolPoints;
    private NavMeshAgent agent;
    private Animator animator;
    private PlayerWeapon weapon;
    private int currentPoint;
    private float noticeClock = 0f;
    private float forgetClock = 0f;
    private bool isSeeingPlayer = false;
    
    public Transform player { get; private set; }
    public bool canMove { get => !agent.isStopped; set => agent.isStopped = !value; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<PlayerWeapon>();

        AIManager.enemies.Add(this);

        Patrol();
    }

    void Update()
    {
        NoticePlayer();
        AttackPlayer();

        if (AIManager.player != null)
            agent.SetDestination(AIManager.playerLastKnownPosition);
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
                        forgetClock = 0f;
                        AIManager.SoundTheAlarm();
                    }
                    else
                    {
                        isSeeingPlayer = true;
                        transform.LookAt(cols[0].transform);

                        noticeClock += Time.deltaTime * (distanceOfView / Vector3.Distance(transform.position, cols[0].transform.position));
                    }
                    
                    return;
                }
            }
        }

        if (AIManager.alarm && player != null && forgetClock < forgetTime)
        {
            forgetClock += Time.deltaTime;
            return;
        }

        player = null;
        isSeeingPlayer = false;
        noticeClock = 0f;
    }
    
    private void AttackPlayer()
    {
        if (AIManager.player != null && canMove && Physics.OverlapSphere(transform.position, attackRange, playerLayer.value).Length != 0)
        {
            transform.LookAt(AIManager.player);
            animator.SetTrigger("attack");
        }
    }

    public void FindPlayer()
    {
        StartCoroutine(FindingPlayer());
    }

    IEnumerator FindingPlayer()
    {
        float seekClock = 0f;

        while (AIManager.lookingForPlayer)
        {
            if (agent.velocity == Vector3.zero)
            {
                while (seekClock < waitTime && AIManager.lookingForPlayer)
                {
                    seekClock += Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }

                seekClock = 0f;

                if (AIManager.player == null)
                {
                    GoToRandomPoint();

                    yield return new WaitUntil (
                        () => agent.velocity == Vector3.zero || !AIManager.lookingForPlayer
                    );
                }
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }

    private void GoToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * findingRadius;
        randomDirection += AIManager.playerLastKnownPosition;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, agent.height * 2, 1);
        Vector3 finalPosition = hit.position;

        agent.SetDestination(finalPosition);
    }

    public void Patrol()
    {
        StartCoroutine(Patroling());
    }

    IEnumerator Patroling()
    {
        float waitClock = 0f;

        while (!AIManager.alarm)
        {
            if (isSeeingPlayer)
            {
                agent.isStopped = true;
                yield return new WaitUntil(() => !isSeeingPlayer || AIManager.alarm);
                agent.isStopped = false;

                continue;
            }

            agent.SetDestination(patrolPoints[currentPoint].position);
            currentPoint = (currentPoint + 1) % patrolPoints.Length;

            yield return new WaitWhile(() => agent.velocity == Vector3.zero);
            yield return new WaitUntil(() => agent.velocity == Vector3.zero || AIManager.alarm || isSeeingPlayer);

            while (waitClock < waitTime && !AIManager.alarm && !isSeeingPlayer)
            {
                waitClock += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            waitClock = 0f;
        }
    }

    public void SetAlarmedState()
    {
        animator.SetBool("isAlarmed", true);
    }

    public void UnsetAlarmedState()
    {
        animator.SetBool("isAlarmed", false);
    }

    public void OnAttackStartEvent()
    {
        weapon.StartDamaging();
    }

    public void OnAttackEndEvent()
    {
        weapon.StopDamaging();
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

        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, fieldOfView / 2f, 0f)  * (transform.forward) * distanceOfView);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, -fieldOfView / 2f, 0f) * (transform.forward) * distanceOfView);
    }
}