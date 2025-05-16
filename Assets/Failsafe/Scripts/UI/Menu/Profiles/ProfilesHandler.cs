using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ProfilesHandler : MonoBehaviour
{
    public Profile profilePrefab;

    public GameObject ProfilesContainerGO;

    public List<Profile> profiles = new List<Profile>();

    [Inject] SaveManager saveManager;
     [Inject] DiContainer _container;


    public void UpdateProfilesList()
    {
        profiles.Clear();
        profiles.AddRange(ProfilesContainerGO.GetComponentsInChildren<Profile>());
    }

    public void AddToProfilesList(Profile _newProfile)
    {
        foreach (var item in profiles)
        {
            if(item.profileID == _newProfile.profileID)
            {
                _newProfile.profileID++;
            }
            else
            {
                break;
            }
        }
        
        
        _newProfile.transform.SetSiblingIndex(_newProfile.profileID-1);
        _newProfile.UpdateProfileUI();
        UpdateProfilesList();

        saveManager.SaveAll();
    }

   

    public void RemoveFromProfilesList(Profile _profile)
    {
        profiles.Remove(_profile);
        
        saveManager.SaveAll();
    }

    #region SAVE AND LOAD

    public void Save(ref ProfileDATA[] data)
    {
        data = new ProfileDATA[profiles.Count];
        for (int i = 0; i < profiles.Count; i++)
        {
            data[i] = new ProfileDATA(profiles[i]);
        }
    }

    public void Load(ProfileDATA[] data)
    {
        foreach (var item in data)
        {
            Profile newProfile = _container.InstantiatePrefabForComponent<Profile>(profilePrefab, ProfilesContainerGO.transform);
            //Profile newProfile = Instantiate(profilePrefab, ProfilesContainerGO.transform);
            newProfile.LoadProfile(item);            
            newProfile.transform.SetSiblingIndex(newProfile.profileID-1);
            UpdateProfilesList();
            newProfile.data = item;
            
        }
        
    }

    #endregion
}
