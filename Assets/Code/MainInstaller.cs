using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private UIInventory uiInventory;

    public override void InstallBindings()
    {
        Container.BindInstance(uiInventory).AsSingle();

        Container.Bind<Inventory>().FromNew().AsTransient();
    }
}