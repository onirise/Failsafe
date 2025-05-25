using System.Collections.Generic;
using UnityEngine;

public enum SaveState
{
    Save,
    Load
}
public class GameplaySavesHandler : MonoBehaviour
{
    public GameObject gameplaySavesGO;
    public List<GameplaySave> gameplaySaves = new List<GameplaySave>();
    public Profile profileParent;

    public SaveState saveState = SaveState.Load;

   

    void Start()
    {
        /*
        gameplaySaves[0].SetSaveName("start_autosave", -1);
        gameplaySaves[0].UpdateGameplaySaveUI(true);
        

        for (int i = 1; i < gameplaySaves.Count; i++)
        {
            gameplaySaves[i].SetSaveName("gameplaySaveName", i);
            gameplaySaves[i].UpdateGameplaySaveUI();
        }*/
    }
   

    public void SetSavesFromSelectedProfile(GameplaySaveDATA[] _gameplaySaveDATA)
    {   
        for (int i = 0; i < gameplaySaves.Count; i++)
        {
            gameplaySaves[i].DATA = _gameplaySaveDATA[i];
            gameplaySaves[i].UpdateGameplaySaveUI();
        }
       
    }

    public void OpenGSavesWindow(Profile _selectedProfile, SaveState _newSaveState)
    {
        gameplaySavesGO.SetActive(true);
        profileParent = _selectedProfile;
        saveState = _newSaveState;
    }
    
   
}
