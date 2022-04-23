using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Actions/HealAction", fileName = "HealAction")]
public class HealAction : ItemAction
{
    [SerializeField] private int amount = 20;

    override public void Use(Transform actor)
    {
        actor.GetComponent<Health>().TakeHealing(amount);
    }
}
