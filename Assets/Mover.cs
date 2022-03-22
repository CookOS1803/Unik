using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newSpeedX = Input.GetAxis("Horizontal") * speed;
        float newSpeedZ = Input.GetAxis("Vertical") * speed;

        rb.velocity = new Vector3(newSpeedX, rb.velocity.y, newSpeedZ);
    }
}
