using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage = 30;
    [SerializeField] private float damageRadius = 0.5f;
    [SerializeField] private Vector3 secondPoint;
    [SerializeField] private LayerMask reactingLayer;
    private bool isDamaging = false;

    public void StartDamaging()
    {
        isDamaging = true;
    }

    public void StopDamaging()
    {
        isDamaging = false;
    }

    void Update()
    {
        if (isDamaging)
        {
            var cols = Physics.OverlapCapsule(transform.position, transform.TransformPoint(secondPoint), damageRadius, reactingLayer);

            if (cols.Length != 0)
            {
                OnHit(cols[0]);

                StopDamaging();
            }
        }
    }

    protected virtual void OnHit(Collider collider)
    {
        var health = collider.GetComponent<Health>();
        health?.TakeDamage(damage);

        var particles = collider.GetComponent<ParticleSystem>();

        if (particles != null)
        {
            //var shape = particles.shape;

            //Vector3 direction = collider.transform.position - transform.position;
            //
            //shape.rotation = new Vector3(shape.rotation.x, collider.transform.eulerAngles.y - Vector3.SignedAngle(collider.transform.forward, direction, Vector3.up), shape.rotation.z);

            particles.Play();
        }    
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = isDamaging ? Color.green : Color.red;

        Gizmos.DrawWireSphere(transform.position, damageRadius);
        Gizmos.DrawWireSphere(transform.TransformPoint(secondPoint), damageRadius);
    }
}
