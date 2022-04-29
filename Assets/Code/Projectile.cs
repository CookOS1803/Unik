using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifeTime = 5f;
    private float lifeClock = 0f;

    void Update()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);

        lifeClock += Time.deltaTime;
        if (lifeClock > lifeTime)
            Destroy(gameObject);
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
