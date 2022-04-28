using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float alarmTime = 4f;
    private float alarmClock = 0f;
    private static AIManager instance;
    public static Vector3 playerLastKnownPosition { get; private set; }
    public static List<EnemyController> enemies { get; private set; }
    public static bool alarm { get; private set; } = false;
    
    public static Transform player { get; private set; }
    public static bool lookingForPlayer => alarm && player == null;

    void Awake()
    {
        if (instance != null)
            Debug.LogError("AIManager object already exists");
        else
        {
            instance = this;
            enemies = new List<EnemyController>();
        }
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

    public static void SoundTheAlarm()
    {
        if (!alarm)
        {
            alarm = true;
            
            instance.StartCoroutine(instance.ManagingAlarm());
            instance.StartCoroutine(instance.ExecutingFind());
        }
    }

    IEnumerator ManagingAlarm()
    {
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
            e.Patrol();
        }
    }
}
