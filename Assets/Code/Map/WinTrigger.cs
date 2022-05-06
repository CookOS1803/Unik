using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject restartButton;
    [Inject] private new CameraController camera;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            Destroy(player.gameObject);
            camera.OnWin();
            restartButton.SetActive(true);
        }
    }
}
