using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    private Health playerHealth;

    [Inject]
    void SetHealth(PlayerController player)
    {
        playerHealth = player.GetComponent<Health>();

        playerHealth.onChange += Event_UpdateBar;

    }

    void Event_UpdateBar(object sender, EventArgs e) => UpdateBar();

    void UpdateBar()
    {
        bar.fillAmount = playerHealth.normalizedHealth;
    }
}
