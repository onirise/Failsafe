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

    public SaveSlotDATA[] SaveSlotsDATAs = new SaveSlotDATA[3] { new SaveSlotDATA(), new SaveSlotDATA(), new SaveSlotDATA() };

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
public class SaveSlotDATA
{
    public int Seed;

    public float Time;

    public Vector3 PlayerPosition;

    public string ScreenshotLink;

    public bool LastSave;

    public bool IsEmpty;

    public SaveSlotDATA()
    {
        Seed = 0;
        Time = 0;
        PlayerPosition = Vector3.zero;
        ScreenshotLink = "";
        LastSave = false;
        IsEmpty = true;
    }


}