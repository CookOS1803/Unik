using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] private float speed = 4f;
    [SerializeField, Min(0f)] private float lifeTime = 5f;
    private float lifeClock = 0f;

    void Update()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);

        lifeClock += Time.deltaTime;
        if (lifeClock > lifeTime)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();
        health?.TakeDamage(damage);

        var enemy = other.GetComponent<EnemyController>();
        enemy?.Stun();

        var particles = other.GetComponent<ParticleSystem>();
        if (particles != null)
        {
            particles.Emit(6);
        }

        Destroy(gameObject);
    }


}
