using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float alarmTime = 4f;
    [SerializeField, Min(0f)] private float forgetTime = 0.5f;
    private float alarmClock = 0f;
    private float forgetClock = 0f;
    private static AIManager instance;
    private static Transform _player;
    public static Vector3 playerLastKnownPosition { get; private set; }
    public static List<EnemyController> enemies { get; private set; }
    public static bool alarm { get; private set; } = false;
    
    public static Transform player { get; private set; }

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
        if (alarm)
        {
            Debug.Log(forgetClock);
            if (alarmClock >= alarmTime)
            {
                alarmClock = 0f;
                alarm = false;
            }
            else if (player == null)
            {
                alarmClock += Time.deltaTime;
            }
            else
            {
                alarmClock = 0f;
            }

        }
 
        foreach (var e in enemies)
        {
            if (e.player != null)
            {
                SetPlayer(e.player);
                return;
            }
        }
        
        if (alarm)
        {
            if (forgetClock < forgetTime && player != null)
            {
                playerLastKnownPosition = player.position;
                forgetClock += Time.deltaTime;
                return;
            }
            else
                forgetClock = 0f;

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
            instance.StartCoroutine(instance.ExecutingFind());
        }
    }

    IEnumerator ExecutingFind()
    {
        while (alarm)
        {
            if (player)
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
