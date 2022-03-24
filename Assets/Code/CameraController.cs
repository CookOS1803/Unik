using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float sensetivity = 2f;
	public float smoothing = 2f;
	private Vector2 mouseLook;
	private Vector2 smoothV;
	private Transform character;

	void Start()
	{
		character = transform.parent;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		mouseDelta = Vector2.Scale(mouseDelta, new Vector2 (sensetivity * smoothing, sensetivity * smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
		mouseLook += smoothV;

		mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);

		transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		character.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
	}
}
