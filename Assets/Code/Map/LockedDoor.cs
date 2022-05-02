using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private ItemData key;

    public override void Use(Transform user)
    {
        if (isClosed)
        {
            var player = user.GetComponent<PlayerController>();
            var inventory = player?.inventory;

            if (inventory != null)
            {
                foreach (Item i in inventory)
                {
                    if (i?.data == key)
                    {
                        Open();
                        break;
                    }
                }
            }
        }
        else
        {
            Close();
        }
    }
}