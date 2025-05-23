using UnityEngine;
using Zenject;

public class NewProfileButton : MonoBehaviour
{
    [Inject] ProfilesHandler profilesHandler;
    [Inject] DiContainer _container;
    public Profile profilePrefab;
    public void CreateProfile()
    {
        Profile newProfile = _container.InstantiatePrefabForComponent<Profile>(profilePrefab, profilesHandler.ProfilesContainerGO.transform);
        //newProfile.DATA = new ProfileDATA();
        newProfile.SetNewLocaleEntry();
        newProfile.UpdateProfileUI();
        profilesHandler.AddToProfilesList(newProfile);
        if(profilesHandler.GetSelectedProfile() == null)
            newProfile.SelectCLickedProfile();
        
    }
}
