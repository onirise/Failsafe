using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;


public static class SaveManager
{

    public static ProfilesHandler profilesHandler;
    public static string SavePath()
    {
        return Path.Combine(Application.persistentDataPath, "profiles.json");
    }


    public static void SaveAll()
    {

        ProfileSaveDATA _saveDATA = profilesHandler.ToSaveData();

        string json = JsonUtility.ToJson(_saveDATA);
        File.WriteAllText(SavePath(), json);
    }


    public static void LoadAll()
    {
        if (File.Exists(SavePath()))
        {

            string json = File.ReadAllText(SavePath());
            ProfileSaveDATA _saveDATA = JsonUtility.FromJson<ProfileSaveDATA>(json);
            if (_saveDATA.profileDATAs.Length > 0)
            {
                profilesHandler.Load(_saveDATA);
            }
            else
            {
                ProfileDATA[] fallbackProfileDATAs = new ProfileDATA[] { new ProfileDATA() };
                ProfileSaveDATA fallbackSaveDATA = new ProfileSaveDATA(fallbackProfileDATAs, 0);
                profilesHandler.Load(fallbackSaveDATA);
            }

        }


    }

}
