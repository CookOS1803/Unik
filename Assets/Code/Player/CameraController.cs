using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 1f;
    [SerializeField] private float mouseFactor = 1f;
    [SerializeField] private float additionalMouseFactor = 10f;
    [SerializeField] private Vector3 offset;
    private float currentMouseFactor;

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
}
