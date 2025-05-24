using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ProfilesHandler : MonoBehaviour
{
    public Profile profilePrefab;

    public GameObject ProfilesContainerGO;

    public List<Profile> profiles = new List<Profile>();
    public List<ProfileDATA> profiles1 = new List<ProfileDATA>();

    //[Inject] SaveManager saveManager;
    [Inject] DiContainer _container;

    public ProfileDATA CreateNewProfile()
    {
        ProfileDATA newProfileData = new ProfileDATA();
        profiles1.Add(newProfileData);
        return newProfileData;
    }

    public void UpdateProfilesList()
    {
        profiles.Clear();
        profiles.AddRange(ProfilesContainerGO.GetComponentsInChildren<Profile>());
    }

    public void AddToProfilesList(Profile _newProfile)
    {
        
        _newProfile.UpdateProfileUI();
        UpdateProfilesList();

        SaveManager.SaveAll();
    }

   

    public void RemoveFromProfilesList(Profile _profile)
    {
        profiles.Remove(_profile);
        
        SaveManager.SaveAll();
    }

    public Profile GetSelectedProfile()
    {
        foreach (var item in profiles)
        {
            if(item.DATA.selected == true)
                return item;
        }
        return null;
    }

    #region SAVE AND LOAD

    public void Save(ref ProfileDATA[] data)
    {
        data = new ProfileDATA[profiles.Count];
        for (int i = 0; i < profiles.Count; i++)
        {
            data[i] = profiles[i].DATA;
            data[i].gameplaySaveDATAs = profiles[i].DATA.gameplaySaveDATAs;
        }
    }

    public void Load(ProfileDATA[] data)
    {
        
        profiles1.AddRange(data);
        foreach (var item in data)
        {
            Profile newProfile = _container.InstantiatePrefabForComponent<Profile>(profilePrefab, ProfilesContainerGO.transform);
            //Profile newProfile = Instantiate(profilePrefab, ProfilesContainerGO.transform);
            //newProfile.LoadProfile(item);            
            
            newProfile.SetDATA(item);
            UpdateProfilesList();
            newProfile.transform.SetSiblingIndex(profiles.Count-1);
            
        }
        
    }

    #endregion
}
