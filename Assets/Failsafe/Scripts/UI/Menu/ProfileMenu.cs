using UnityEngine;
using Zenject;
public class ProfileMenu : BaseMenu
{
    [Inject] ProfilesHandler profilesHandler;
    [Inject] TabletHandler tabletHandler;
    [Inject] DiContainer _container;
    
    public GameObject ProfilesContainerGO;
    public Profile profilePrefab;
   
    public GameObject mainMenuMainGO;

    void Start()
    {
        RerenderProfiles();
    }

    private void RerenderProfiles()
    {
        

        for (int i = 0; i < profilesHandler.profiles1.Count; i++)
        {
            RenderProfile(profilesHandler.profiles1[i], i);
        }
    }

    private void RenderProfile(ProfileDATA item, int index)
    {
        Profile newProfile = _container.InstantiatePrefabForComponent<Profile>(profilePrefab, ProfilesContainerGO.transform);
        newProfile.SetDATA(item);
    }

    public void CreateProfile(ProfileDATA _data)
    {
        Profile newProfile = _container.InstantiatePrefabForComponent<Profile>(profilePrefab, profilesHandler.ProfilesContainerGO.transform);
        newProfile.SetDATA(_data);
       
        newProfile.SetNewLocaleEntry();
        newProfile.UpdateProfileUI();
        profilesHandler.AddToProfilesList(newProfile);
        if(profilesHandler.GetSelectedProfile() == null)
            newProfile.SelectCLickedProfile();
        
    }

    public void OnCreateNewProfile()
    {
        
        RenderProfile(profilesHandler.CreateNewProfile(), profilesHandler.profiles1.Count);
    }

    public void OnProfilesClose()
    {
        gameObject.SetActive(false);
        mainMenuMainGO.SetActive(true);
        
    } 



}
