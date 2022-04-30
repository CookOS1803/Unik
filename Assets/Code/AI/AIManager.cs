using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public event Action onAlarmEnable;
    public event Action onAlarmDisable;
    public event Action onAlarmClockChange;

    [SerializeField, Min(0f)] private float alarmTime = 4f;
    private float _alarmClock = 0f;
    private bool _alarm = false;
    private float alarmClock
    {
        get => _alarmClock;
        set
        {
            _alarmClock = value;

            onAlarmClockChange?.Invoke();
        }
    }

    public Vector3 playerLastKnownPosition { get; private set; }
    public List<EnemyController> enemies { get; private set; }
    public Transform player { get; private set; }
    public bool alarm => _alarm;    
    public bool lookingForPlayer => alarm && player == null;
    public float normalizedAlarmClock => alarmClock / alarmTime;
    

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
            EnableAlarm();
            
            StartCoroutine(ManagingAlarm());
            StartCoroutine(ExecutingFind());

            foreach (var e in enemies)
            {
                e.SetAlarmedState();
            }
        }
    }

    private void EnableAlarm()
    {
        _alarm = true;
        
        onAlarmEnable?.Invoke();
    }

    private void DisableAlarm()
    {
        _alarm = false;
        
        onAlarmDisable?.Invoke();
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
                DisableAlarm();
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
