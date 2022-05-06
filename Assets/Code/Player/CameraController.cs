using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float smoothing = 1f;
    [SerializeField] private float mouseFactor = 1f;
    [SerializeField] private float additionalMouseFactor = 10f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 winPosition = new Vector3(-13.5f, 6.28f, 2.36f);
    [SerializeField] private Vector3 winEulerAngles = new Vector3(49.4f, 45f, 0f);
    private Transform target;
    private float currentMouseFactor;

    [Inject]
    void SetTarget(PlayerController player)
    {
        target = player.transform;
    }

    void Start()
    {
        currentMouseFactor = mouseFactor;
    }

    void Update()
    {
        currentMouseFactor = Input.GetButton("View") ? additionalMouseFactor : mouseFactor;
    }

    void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.x /= Screen.width;
        mousePosition.y /= Screen.height;
        mousePosition.z = mousePosition.y;
        mousePosition.y = 0f;

        mousePosition.x -= 0.5f;
        mousePosition.z -= 0.5f;
        
        Vector3 targetCamPos = target.position + offset + mousePosition * currentMouseFactor;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    public void OnWin()
    {
        transform.position = winPosition;
        transform.eulerAngles = winEulerAngles;

        Destroy(this);
    }
}
