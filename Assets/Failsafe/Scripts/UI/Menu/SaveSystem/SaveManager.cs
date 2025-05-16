using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;


public class SaveManager //: IInitializable
{
    static SaveDATA _saveDATA = new SaveDATA();
    
    [Inject] ProfilesHandler profilesHandler;

    public static string SavePath()
    {        
        return Path.Combine(Application.persistentDataPath, "profiles.json");
    }

   
    public void SaveAll()
    {
        profilesHandler.Save(ref _saveDATA.profileDATAs);

        string json = JsonUtility.ToJson(_saveDATA);
        File.WriteAllText(SavePath(), json);
    }

    
    public void LoadAll()
    {
        if (File.Exists(SavePath()))
        {
            string json = File.ReadAllText(SavePath());
            _saveDATA = JsonUtility.FromJson<SaveDATA>(json);
            profilesHandler.Load(_saveDATA.profileDATAs);
        }

       
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }
}
