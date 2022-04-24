using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] private float speed = 4f;

    void Update()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }


}
