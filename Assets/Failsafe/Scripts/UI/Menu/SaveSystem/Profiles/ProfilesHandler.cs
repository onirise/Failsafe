using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class ProfilesHandler
{


    public static List<ProfileDATA> Profiles = new List<ProfileDATA>();
    public static ProfileDATA SelectedProfile;
    public static UnityAction OnProfilesChanged;




    public static ProfileDATA CreateNewProfile()
    {
        ProfileDATA newProfileData = new ProfileDATA();
        Profiles.Add(newProfileData);
        SelectedProfile = GetSelectedProfile() ? SelectedProfile : newProfileData;
        SaveManager.SaveAll();
        OnProfilesChanged?.Invoke();
        return newProfileData;

    }
    public static void RemoveFromProfilesList(int _index)
    {
        bool selectedIsDeleted = IsSelectedProfile(_index);
        Profiles.RemoveAt(_index);
        if (selectedIsDeleted && Profiles.Count > 0)
            SelectedProfile = Profiles[Profiles.Count - 1];
        else if (Profiles.Count == 0)
            SelectedProfile = null;
        OnProfilesChanged?.Invoke();
        SaveManager.SaveAll();
    }


    public static bool GetSelectedProfile()
    {
        return SelectedProfile != null;
    }

    public static void SetSelectedProfile(int _index)
    {
        SelectedProfile = Profiles[_index];
        SaveManager.SaveAll();
        OnProfilesChanged?.Invoke();
    }

    public static bool IsSelectedProfile(int _index)
    {
        return SelectedProfile == Profiles[_index];
    }
    public static bool IsSelectedProfileIsNew(int _index)
    {
        return !IsSelectedProfile(_index) && SelectedProfile.IsNew;
    }

    public static int GetSelectedProfileIndex()
    {
        return Profiles.IndexOf(SelectedProfile);
    }

    public static bool IsProfilesGreaterThanZero()
    {
        return Profiles.Count > 0;
    }



    #region SAVE AND LOAD

    public static ProfileSaveDATA ToSaveData()
    {
        return new ProfileSaveDATA(Profiles.ToArray(), GetSelectedProfileIndex());
    }

    public static void Load(ProfileSaveDATA data)
    {
        Profiles.Clear(); // костыль. из-за того, что это теперь статик-класс - лист не очищается, а всегда заполнен 
        // и туда второй раз грузятся профили если в каком-то скрипте в Start() вызван SaveManager.LoadAll() 
        Profiles.AddRange(data.ProfileDATAs);
        SetSelectedProfile(data.SelectedProfileIndex);
    }

    #endregion
}
