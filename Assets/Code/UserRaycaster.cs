using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UserRaycaster
{
    public static bool IsBlockedByUI()
    {
        var p = new PointerEventData(EventSystem.current);
        p.position = Input.mousePosition;
        var list = new List<RaycastResult>();

        EventSystem.current.RaycastAll(p, list);

        return list.Count != 0;
    }

    
}
