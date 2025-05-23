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
    public int profileID = 1;

    public bool isNew = true;

    public GameplaySaveDATA[] gameplaySaveDATAs = new GameplaySaveDATA[3] {new GameplaySaveDATA(), new GameplaySaveDATA(), new GameplaySaveDATA()};
   
   

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