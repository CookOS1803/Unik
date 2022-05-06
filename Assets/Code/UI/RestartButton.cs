using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class RestartButton : MonoBehaviour
{
    [Inject] private PlayerController player;
    private GameObject restartButton;

    void Start()
    {
        restartButton = GetComponentInChildren<Button>().gameObject;
        player.onDeath += ActivateRestartButton;

        restartButton.SetActive(false);
    }

    void ActivateRestartButton()
    {
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
