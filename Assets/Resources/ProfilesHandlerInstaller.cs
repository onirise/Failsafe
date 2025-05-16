using UnityEngine;
using Zenject;

public class ProfilesHandlerInstaller : MonoInstaller
{
    [SerializeField]
    ProfilesHandler profilesHandlerInstance;

    public override void InstallBindings()
    {
        Container.Bind<ProfilesHandler>().FromInstance(profilesHandlerInstance).AsSingle().NonLazy();
        //Container.Bind<SaveManager>().AsSingle().NonLazy(); // НАВЕРНОЕ НЕ ПОНАДОБИТСЯ УЖЕ
    }
}