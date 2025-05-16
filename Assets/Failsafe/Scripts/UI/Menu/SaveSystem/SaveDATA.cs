using System;
using UnityEngine;

[Serializable]
public class SaveDATA 
{
    public ProfileDATA[] profileDATAs;


}

[Serializable]
public class ProfileDATA
{
    public bool selected;
    public int localeEntryIndex;
    public int profileID;

    public GameplaySaveDATA[] gameplaySaveDATAs;

    public ProfileDATA(Profile _profile)
    {
        selected = _profile.selected;
        localeEntryIndex = _profile.localeEntryIndex;
        profileID = _profile.profileID;
    }

}

[Serializable]
public class GameplaySaveDATA
{
    public int seed;

    public float time;

    public Vector3 playerPosition;

    public string screenshotLink;

    public bool selected;

    public GameplaySaveDATA(GameplaySave _gSave)
    {
        //selected = _gSave.selected;
        //screenshotLink = _gSave.screenshotLink;
       // localeEntryIndex = _gSave.localeEntryIndex;
       // profileID = _gSave.profileID;
    }

}