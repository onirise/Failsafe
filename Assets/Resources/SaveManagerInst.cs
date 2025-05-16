using UnityEngine;
using Zenject;


public class SaveManagerInst : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<SaveManager>().AsSingle().NonLazy();
    }
}