using UnityEngine;
using Zenject;

public class InputSystemInstaller : MonoInstaller
{
    [SerializeField] private DynamicJoystick _dynamicJoystick;
    public override void InstallBindings()
    {
        Container.Bind<IJoystick>().FromInstance(_dynamicJoystick).AsSingle().NonLazy();
    }
}