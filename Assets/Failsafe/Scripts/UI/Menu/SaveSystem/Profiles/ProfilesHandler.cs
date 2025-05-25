using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
public class ProfilesHandler : MonoBehaviour
{


    public List<ProfileDATA> profiles1 = new List<ProfileDATA>();
    public ProfileDATA selectedProfile;
    public UnityAction OnProfilesChanged;



    public ProfileDATA CreateNewProfile()
    {
        ProfileDATA newProfileData = new ProfileDATA();
        profiles1.Add(newProfileData);
        selectedProfile = GetSelectedProfile() ? selectedProfile : newProfileData;
        SaveManager.SaveAll();
        OnProfilesChanged?.Invoke();
        return newProfileData;

    }
    public void RemoveFromProfilesList(int _index)
    {
        bool selectedIsDeleted = IsSelectedProfile(_index);
        profiles1.RemoveAt(_index);
        if (selectedIsDeleted && profiles1.Count > 0)
            selectedProfile = profiles1[profiles1.Count - 1];
        OnProfilesChanged?.Invoke();
        SaveManager.SaveAll();
    }


    public bool GetSelectedProfile()
    {
        return selectedProfile != null;
    }

    public void SetSelectedProfile(int _index)
    {
        selectedProfile = profiles1[_index];
        SaveManager.SaveAll();
        OnProfilesChanged?.Invoke();
    }

    public bool IsSelectedProfile(int _index)
    {
        return selectedProfile == profiles1[_index];
    }

    public int GetSelectedProfileIndex()
    {
        return profiles1.IndexOf(selectedProfile);
    }

    #region SAVE AND LOAD

    public ProfileSaveDATA ToSaveData()
    {
        return new ProfileSaveDATA(profiles1.ToArray(), GetSelectedProfileIndex());
    }

    public void Load(ProfileSaveDATA data)
    {

        profiles1.AddRange(data.profileDATAs);
        SetSelectedProfile(data.selectedProfileIndex);
    }

    #endregion
}
