using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float alarmTime = 4f;
    public Vector3 playerLastKnownPosition { get; private set; }
    public List<EnemyController> enemies { get; private set; }
    public bool alarm { get; private set; } = false;
    
    public Transform player { get; private set; }
    public bool lookingForPlayer => alarm && player == null;

    void Awake()
    {
        enemies = new List<EnemyController>();
    }

    void Update()
    {        
        foreach (var e in enemies)
        {
            if (e.player != null)
            {
                SetPlayer(e.player);
                return;
            }
        }                
        
        UnsetPlayer();
    }

    private void SetPlayer(Transform p)
    {
        player = p;
        playerLastKnownPosition = player.position;
    }

    private void UnsetPlayer()
    {
        player = null;
    }

    public void SoundTheAlarm()
    {
        if (!alarm)
        {
            alarm = true;
            
            StartCoroutine(ManagingAlarm());
            StartCoroutine(ExecutingFind());

            foreach (var e in enemies)
            {
                e.SetAlarmedState();
            }
        }
    }

    IEnumerator ManagingAlarm()
    {
        float alarmClock = 0f;

        while (alarm)
        {
            yield return new WaitUntil(() => player == null);

            while (alarmClock < alarmTime)
            {
                if (player == null)
                {
                    alarmClock += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    alarmClock = 0f;
                    break;
                }
            }

            if (player == null)
                alarm = false;
        }

        alarmClock = 0f;
    }

    IEnumerator ExecutingFind()
    {
        while (alarm)
        {
            if (player != null)
            {
                yield return new WaitUntil(() => player == null);

                foreach (var e in enemies)
                {
                    e.FindPlayer();
                }
            }
            else
                yield return new WaitForEndOfFrame();
        }

        foreach (var e in enemies)
        {
            e.UnsetAlarmedState();
            e.Patrol();
        }
    }
}
