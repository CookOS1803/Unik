using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float damageRadius = 0.5f;
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
            var cols = Physics.OverlapSphere(transform.position, damageRadius);

            if (cols.Length != 0)
            {
                Debug.Log(cols[0].name);
                StopDamaging();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isDamaging ? Color.green : Color.red;

        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
