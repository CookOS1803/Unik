using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float alarmTime = 4f;
    private float alarmClock = 0f;
    private static AIManager instance;
    private static Transform _player;
    public static Vector3 playerLastKnowPosition { get; private set; }
    public static List<EnemyController> enemies { get; private set; }
    public static bool alarm { get; private set; } = false;
    
    public static Transform player
    {
        get => _player;
        private set
        {
            if (_player != null)
                playerLastKnowPosition = _player.position;
            
            _player = value;
        }
    }

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
            Debug.Log(alarmClock);

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
                alarmClock = 0f;
        }


        foreach (var e in enemies)
        {
            if (e.player != null)
            {
                player = e.player;
                return;
            }
        }

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
            }
            else
            {
                yield return new WaitForEndOfFrame();
                continue;    
            }

            foreach (var e in enemies)
            {
                e.FindPlayer();
            }            
        }
    }
}
