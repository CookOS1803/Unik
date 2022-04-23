using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float damageRadius = 0.5f;

    public void DoDamage()
    {
        Instantiate(ItemAssets.assets[0].prefab, transform.position, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
