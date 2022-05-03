using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Actions/ThrowProjectileAction", fileName = "ThrowProjectileAction")]
public class ThrowProjectileAction : ItemAction
{
    [SerializeField] private GameObject projectilePrefab;

    public override void Use(Transform actor)
    {
        Instantiate(projectilePrefab, actor.position + Vector3.up + actor.forward * 0.5f, actor.rotation);
    }
}
