using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVRenderer : MonoBehaviour
{
    [SerializeField, Min(2)] private int rayCount = 2;
    private Mesh mesh;
    private EnemyController enemy;
    private float angleInc => enemy.fieldOfView / rayCount;
    private float scaleFactor => 1f / transform.parent.localScale.z;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        enemy = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];
        float angle = -enemy.fieldOfView / 2 - transform.eulerAngles.y;

        vertices[0] = Vector3.zero;        
        vertices[1] = ApplyObstacle(GetVertex(angle));

        angle += angleInc;

        int vertexIndex = 2;
        int triangleIndex = 0;

        for (int i = 1; i <= rayCount; i++)
        {
            vertices[vertexIndex] = ApplyObstacle(GetVertex(angle));

            triangles[triangleIndex++] = 0;
            triangles[triangleIndex++] = vertexIndex - 1;
            triangles[triangleIndex++] = vertexIndex;

            vertexIndex++;
            angle += angleInc;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }

    private Vector3 GetVertex(float angle)
    {
        return Quaternion.Euler(0f, angle, 0f) * transform.forward * enemy.distanceOfView * scaleFactor;
    }

    private Vector3 ApplyObstacle(Vector3 vertex)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, transform.position + transform.parent.TransformDirection(vertex / scaleFactor), out hit))
        {
            vertex *= Vector3.Distance(transform.position, hit.point) / enemy.distanceOfView;
        }

        return vertex;
    }
}
