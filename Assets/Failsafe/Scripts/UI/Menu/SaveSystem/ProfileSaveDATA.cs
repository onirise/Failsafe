using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

[Serializable]
public class ProfileSaveDATA
{
    public ProfileDATA[] profileDATAs;

    public int selectedProfileIndex;

    public ProfileSaveDATA(ProfileDATA[] _profileDATAs, int _selectedProfileIndex)
    {
        profileDATAs = _profileDATAs;
        selectedProfileIndex = _selectedProfileIndex;
    }

}

[Serializable]
public class ProfileDATA
{

    public int localeEntryIndex = 0;

    public bool isNew = true;

    public GameplaySaveDATA[] gameplaySaveDATAs = new GameplaySaveDATA[3] { new GameplaySaveDATA(), new GameplaySaveDATA(), new GameplaySaveDATA() };

    // public void SetNewLocaleEntry()
    // {
    //     var stringTable = LocalizationSettings.StringDatabase.GetTable("ProfileNamesTable"); 
    //     localeEntryIndex = UnityEngine.Random.Range(0, stringTable.Count);

    // }

    public ProfileDATA()
    {
        localeEntryIndex = 0;
        //SetNewLocaleEntry();
    }


}

[Serializable]
public class GameplaySaveDATA
{
    public int seed;

    public float time;

    public Vector3 playerPosition;

    public string screenshotLink;

    public bool lastSave;

    public bool isEmpty;

    public GameplaySaveDATA()
    {
        seed = 0;
        time = 0;
        playerPosition = Vector3.zero;
        screenshotLink = "";
        lastSave = false;
        isEmpty = true;
    }


}