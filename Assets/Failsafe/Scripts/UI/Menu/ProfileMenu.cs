using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class ProfileMenu : BaseMenu
{

    [SerializeField]
    GameObject _profilesContainerGO;
    [SerializeField]
    Profile _profilePrefab;
    [SerializeField]
    GameObject _mainMenuMainGO;

    void Start()
    {
        RerenderProfiles();
        ProfilesHandler.OnProfilesChanged += RerenderProfiles;
    }

    void RerenderProfiles()
    {
        ClearAllProfiles();
        for (int i = 0; i < ProfilesHandler.Profiles.Count; i++)
        {
            RenderProfile(ProfilesHandler.Profiles[i], i);
        }
    }

    void ClearAllProfiles()
    {
        foreach (var item in _profilesContainerGO.GetComponentsInChildren<Profile>())
        {
            Destroy(item.gameObject);
        }
    }

    void RenderProfile(ProfileDATA item, int _index)
    {
        Profile newProfile = Instantiate(_profilePrefab, _profilesContainerGO.transform);
        newProfile.SetDATA(item, _index);
    }



    public void OnCreateNewProfile()
    {
        ProfilesHandler.CreateNewProfile();
    }

    public void OnProfilesClose()
    {
        gameObject.SetActive(false);
        _mainMenuMainGO.SetActive(true);

    }


}
