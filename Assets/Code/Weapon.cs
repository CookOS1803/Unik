using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage = 30;
    [SerializeField] private float damageRadius = 0.5f;
    [SerializeField] private Vector3 secondPoint;
    [SerializeField] private LayerMask ignoredLayer;
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
            var cols = Physics.OverlapCapsule(transform.position, transform.TransformPoint(secondPoint), damageRadius, ~ignoredLayer);

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

        if (health != null)
            health.TakeDamage(damage);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = isDamaging ? Color.green : Color.red;

        Gizmos.DrawWireSphere(transform.position, damageRadius);
        Gizmos.DrawWireSphere(transform.TransformPoint(secondPoint), damageRadius);
    }
}
