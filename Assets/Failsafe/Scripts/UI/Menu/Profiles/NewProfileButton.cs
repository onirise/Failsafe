using UnityEngine;
using Zenject;

public class NewProfileButton : MonoBehaviour
{
    [Inject] ProfilesHandler profilesHandler;
    public Profile profilePrefab;
    public void CreateProfile()
    {
        Profile newProfile = Instantiate(profilePrefab, profilesHandler.ProfilesContainerGO.transform);
        newProfile.SetNewLocaleEntry();
        newProfile.UpdateProfileUI();
        profilesHandler.AddToProfilesList(newProfile);
        
    }
}
