using System.Collections.Generic;
using UnityEngine;

public enum SaveState
{
    Save,
    Load
}
public class GameplaySavesHandler : MonoBehaviour
{
    public GameObject GameplaySavesGO;
    public List<GameplaySave> GameplaySaves = new List<GameplaySave>();
    //public Profile profileParent;

    public SaveState CurrentSaveState = SaveState.Load;



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
        for (int i = 0; i < GameplaySaves.Count; i++)
        {
            GameplaySaves[i].DATA = _gameplaySaveDATA[i];
            GameplaySaves[i].UpdateGameplaySaveUI();
        }

    }

    public void OpenGSavesWindow(Profile _selectedProfile, SaveState _newSaveState)
    {
        GameplaySavesGO.SetActive(true);
        //profileParent = _selectedProfile;
        CurrentSaveState = _newSaveState;
    }


}
