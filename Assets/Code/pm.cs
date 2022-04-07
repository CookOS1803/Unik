using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pm : MonoBehaviour
{
    public float walkingSpeed = 5f;
    public float runningSpeed = 10f;
    public float jumpSpeed = 5f;
    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float forward = Input.GetAxisRaw("Vertical");
        float right = Input.GetAxisRaw("Horizontal") ;

        rigidbody.velocity += (transform.forward * forward  + transform.right * right) * walkingSpeed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {

    }
}
