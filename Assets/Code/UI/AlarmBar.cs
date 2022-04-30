using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class AlarmBar : MonoBehaviour
{
    [Inject] private AIManager aiManager;
    private TMP_Text text;
    private Image image;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        image = GetComponentInChildren<Image>();

        DisableBar();

        aiManager.onAlarmEnable += EnableBar;
        aiManager.onAlarmDisable += DisableBar;
        aiManager.onAlarmClockChange += UpdateBar;
    }

    void EnableBar()
    {
        text.enabled = true;
        image.enabled = true;
    }

    void DisableBar()
    {
        text.enabled = false;
        image.enabled = false;
    }

    void UpdateBar()
    {
        image.fillAmount = aiManager.normalizedAlarmClock;
    }
}
