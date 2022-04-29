using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyController : MonoBehaviour, IMoveable
{
    [SerializeField, Min(0f)] private float distanceOfView = 10f;
    [SerializeField, Min(0f)] private float attackRange = 1f;
    [SerializeField, Range(0f, 360f)] private float fieldOfView = 90f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField, Min(0f)] private float noticeTime = 2f;
    [SerializeField, Min(0f)] private float unseeFactor = 0.5f;
    [SerializeField, Min(0f)] private float waitTime = 2f;
    [SerializeField, Min(0f)] private float forgetTime = 0.5f;
    [SerializeField, Min(0f)] private float findingRadius = 6f;
    [SerializeField] private Transform[] patrolPoints;
    [Inject] private AIManager aiManager;
    private NavMeshAgent agent;
    private Animator animator;
    private Weapon weapon;
    private int currentPoint;
    private float _noticeClock = 0f;
    private float noticeClock
    {
        get => _noticeClock;
        set
        {
            _noticeClock = value;

            onNoticeClockChange?.Invoke();
        }
    }
    private float forgetClock = 0f;
    private bool isSeeingPlayer = false;
    
    public Transform player { get; private set; }
    public bool canMove { get => !agent.isStopped; set => agent.isStopped = !value; }
    public float normalizedNoticeClock => noticeClock / noticeTime;

    public event Action onNoticeClockChange;
    public event Action onNoticeClockReset;

    void ResetNoticeClock()
    {
        _noticeClock = 0f;

        onNoticeClockReset?.Invoke();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();

        aiManager.enemies.Add(this);

        Patrol();
    }

    void Update()
    {
        NoticePlayer();
        AttackPlayer();

        if (aiManager.player != null)
            agent.SetDestination(aiManager.playerLastKnownPosition);
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
                    if (aiManager.alarm || noticeClock >= noticeTime)
                    {
                        player = hit.transform;
                        forgetClock = 0f;
                        aiManager.SoundTheAlarm();
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

        if (aiManager.alarm && player != null && forgetClock < forgetTime)
        {
            forgetClock += Time.deltaTime;
            return;
        }

        player = null;
        isSeeingPlayer = false;
    }
    
    private void AttackPlayer()
    {
        if (aiManager.player != null && canMove && Physics.OverlapSphere(transform.position, attackRange, playerLayer.value).Length != 0)
        {
            transform.LookAt(aiManager.player);
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

        while (aiManager.lookingForPlayer)
        {
            if (agent.velocity == Vector3.zero)
            {
                while (seekClock < waitTime && aiManager.lookingForPlayer)
                {
                    seekClock += Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }

                seekClock = 0f;

                if (aiManager.player == null)
                {
                    GoToRandomPoint();

                    yield return new WaitUntil (
                        () => agent.velocity == Vector3.zero || !aiManager.lookingForPlayer
                    );
                }
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }

    private void GoToRandomPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * findingRadius;
        randomDirection += aiManager.playerLastKnownPosition;

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

        while (!aiManager.alarm)
        {
            if (isSeeingPlayer)
            {
                agent.isStopped = true;

                yield return new WaitUntil(() => !isSeeingPlayer || aiManager.alarm);

                while (noticeClock > 0f && !aiManager.alarm)
                {
                    noticeClock -= Time.deltaTime * unseeFactor;

                    yield return new WaitForEndOfFrame();
                }

                ResetNoticeClock();

                agent.isStopped = false;

                continue;
            }

            agent.SetDestination(patrolPoints[currentPoint].position);
            currentPoint = (currentPoint + 1) % patrolPoints.Length;

            yield return new WaitWhile(() => agent.velocity == Vector3.zero);
            yield return new WaitUntil(() => agent.velocity == Vector3.zero || aiManager.alarm || isSeeingPlayer);

            while (waitClock < waitTime && !aiManager.alarm && !isSeeingPlayer)
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

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        aiManager.enemies.Remove(this);
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