using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    [SerializeField] private ItemData[] _assets;
    private static ItemAssets instance;
    
    public static ItemData[] assets => instance._assets;

    void Awake()
    {
        if (instance != null)
            Debug.LogError("ItemAssets object already exists");
        else
            instance = this;
    }

}
