using System.Collections.Generic;
using UnityEngine;

public enum SaveState
{
    Save,
    Load
}
public class GameplaySavesMenu : BaseMenu
{
    List<GameplaySave> _gameplaySaves = new List<GameplaySave>();
    SaveState _currentSaveState = SaveState.Load;
    void Start()
    {

        _gameplaySaves[0].SetSaveName("start_autosave", -1);
        _gameplaySaves[0].SetDATA(ProfilesHandler.SelectedProfile.GameplaySaveDATAs[0], true);


        for (int i = 1; i < _gameplaySaves.Count; i++)
        {
            _gameplaySaves[i].SetSaveName("gameplaySaveName", i);
            _gameplaySaves[i].SetDATA(ProfilesHandler.SelectedProfile.GameplaySaveDATAs[i]);
        }
    }


    public void SetSavesFromSelectedProfile(GameplaySaveDATA[] _gameplaySaveDATA)
    {
        for (int i = 0; i < _gameplaySaves.Count; i++)
        {
            _gameplaySaves[i].SetDATA(_gameplaySaveDATA[i]);
            _gameplaySaves[i].SetDATA(ProfilesHandler.SelectedProfile.GameplaySaveDATAs[i]);
        }

    }

    public void OpenGSavesWindow(SaveState _newSaveState)
    {
        gameObject.SetActive(true);

        _currentSaveState = _newSaveState;
    }

    public void OnCLoseWindow()
    {
        gameObject.SetActive(false);
    }
}
