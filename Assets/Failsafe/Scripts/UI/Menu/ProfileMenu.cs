using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
public class ProfileMenu : BaseMenu
{
    [Inject] ProfilesHandler profilesHandler;
    [Inject] DiContainer _container;

    public GameObject ProfilesContainerGO;
    public Profile profilePrefab;

    public GameObject mainMenuMainGO;

    void Start()
    {
        RerenderProfiles();
        profilesHandler.OnProfilesChanged += RerenderProfiles;
    }

    void RerenderProfiles()
    {
        ClearAllProfiles();
        for (int i = 0; i < profilesHandler.profiles1.Count; i++)
        {
            RenderProfile(profilesHandler.profiles1[i], i);
        }
    }

    void ClearAllProfiles()
    {
        foreach (var item in ProfilesContainerGO.GetComponentsInChildren<Profile>())
        {
            Destroy(item.gameObject);
        }
    }

    void RenderProfile(ProfileDATA item, int _index)
    {
        Profile newProfile = _container.InstantiatePrefabForComponent<Profile>(profilePrefab, ProfilesContainerGO.transform);
        newProfile.SetDATA(item, _index);
    }



    public void OnCreateNewProfile()
    {
        profilesHandler.CreateNewProfile();
    }

    public void OnProfilesClose()
    {
        gameObject.SetActive(false);
        mainMenuMainGO.SetActive(true);

    }


}
