using UnityEngine;

abstract public class ItemAction : ScriptableObject
{
    abstract public void Use(Transform actor);
}
