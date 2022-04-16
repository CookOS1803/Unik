using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask floorMask;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 direction = new Vector3(h, 0f, v) * speed;
        direction = Vector3.ClampMagnitude(direction, speed);

        characterController.Move(direction * Time.deltaTime);

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast(camRay, out floorHit, Mathf.Infinity, floorMask.value))
		{
            Debug.Log(floorHit.point);
            transform.LookAt(floorHit.point);
		}
    }
}
