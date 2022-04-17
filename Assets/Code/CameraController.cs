using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 1f;
    [SerializeField] private float mouseFactor = 1f;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
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
        
        Vector3 targetCamPos = target.position + offset + mousePosition * mouseFactor;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
