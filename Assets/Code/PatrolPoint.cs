using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 to = transform.position + transform.forward;
        Gizmos.DrawLine(transform.position, to);
        Gizmos.DrawLine(to, transform.position + (Quaternion.Euler(0f, 10f, 0f) * transform.forward) * 0.7f);
        Gizmos.DrawLine(to, transform.position + (Quaternion.Euler(0f, -10f, 0f) * transform.forward) * 0.7f);
    }
}
