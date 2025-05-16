using System.Collections.Generic;
using UnityEngine;

public class GameplaySavesHandler : MonoBehaviour
{
    public List<GameplaySave> gameplaySaves = new List<GameplaySave>();

    int savesID = 0;
    //public List<GameplaySaveDATA> gameplaySaveDATAs = new List<GameplaySaveDATA>(); 

    void Start()
    {
        gameplaySaves[0].SetSaveName("start_autosave", -1);
        gameplaySaves[0].UpdateGameplaySaveUI();
        

        for (int i = 1; i < gameplaySaves.Count; i++)
        {
            gameplaySaves[i].SetSaveName("gameplaySaveName", i);
            gameplaySaves[i].UpdateGameplaySaveUI();
        }
    }
    public int GetNewSavesID()
    {
        savesID++;
        return savesID;
    }

    public void SetSavesFromProfile(GameplaySaveDATA[] _gameplaySaveDATA)
    {   
        for (int i = 0; i < gameplaySaves.Count; i++)
        {
            gameplaySaves[i].DATA = _gameplaySaveDATA[i];
        }
       
    }

    #region SAVE AND LOAD

    public void Save(ref GameplaySaveDATA[] data)
    {
        data = new GameplaySaveDATA[gameplaySaves.Count];
        for (int i = 0; i < gameplaySaves.Count; i++)
        {
            data[i] = gameplaySaves[i].DATA;
        }
    }

    public void Load(GameplaySaveDATA[] data)
    {
       
        for (int i = 0; i < gameplaySaves.Count; i++)
        {
            gameplaySaves[i].DATA = data[i];
        }
    }

    #endregion
}
