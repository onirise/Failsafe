using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

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

    public bool isNew = true;

    public GameplaySaveDATA[] gameplaySaveDATAs = new GameplaySaveDATA[3] {new GameplaySaveDATA(), new GameplaySaveDATA(), new GameplaySaveDATA()};
   
   public void SetNewLocaleEntry()
    {
        var stringTable = LocalizationSettings.StringDatabase.GetTable("ProfileNamesTable"); 
        localeEntryIndex = UnityEngine.Random.Range(1, stringTable.Count+1);
        
    }

    public ProfileDATA()
    {
        SetNewLocaleEntry();
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