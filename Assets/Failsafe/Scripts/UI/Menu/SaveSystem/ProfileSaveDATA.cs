using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

[Serializable]
public class ProfileSaveDATA
{
    public ProfileDATA[] ProfileDATAs;

    public int SelectedProfileIndex;

    public ProfileSaveDATA(ProfileDATA[] _profileDATAs, int _selectedProfileIndex)
    {
        ProfileDATAs = _profileDATAs;
        SelectedProfileIndex = _selectedProfileIndex;
    }

}

[Serializable]
public class ProfileDATA
{

    public int LocaleEntryIndex = 0;

    public bool IsNew = true;

    public GameplaySaveDATA[] GameplaySaveDATAs = new GameplaySaveDATA[3] { new GameplaySaveDATA(), new GameplaySaveDATA(), new GameplaySaveDATA() };

    // public void SetNewLocaleEntry()
    // {
    //     var stringTable = LocalizationSettings.StringDatabase.GetTable("ProfileNamesTable"); 
    //     localeEntryIndex = UnityEngine.Random.Range(0, stringTable.Count);

    // }

    public ProfileDATA()
    {
        LocaleEntryIndex = 0;
        //SetNewLocaleEntry();
    }


}

[Serializable]
public class GameplaySaveDATA
{
    public int Seed;

    public float Time;

    public Vector3 PlayerPosition;

    public string ScreenshotLink;

    public bool LastSave;

    public bool IsEmpty;

    public GameplaySaveDATA()
    {
        Seed = 0;
        Time = 0;
        PlayerPosition = Vector3.zero;
        ScreenshotLink = "";
        LastSave = false;
        IsEmpty = true;
    }


}