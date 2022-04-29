using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private UIInventory uiInventory;
    [SerializeField] private AIManager aiManager;
    [SerializeField] private PlayerController player;

    public override void InstallBindings()
    {
        Container.Bind<Inventory>().FromNew().AsTransient();
        
        Container.BindInstance(player).AsSingle();
        Container.BindInstance(uiInventory).AsSingle();
        Container.BindInstance(aiManager).AsSingle();
    }
}