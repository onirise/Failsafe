using System;
using System.Collections.Generic;
using UnityEngine;

public enum SaveState
{
    Save,
    Load
}
public class SaveSlotsMenu : BaseMenu
{
    List<SaveSlot> _saveSlots = new List<SaveSlot>();
    SaveState _currentSaveState = SaveState.Load;


    [SerializeField]
    GameObject _saveSlotsContainerGO;
    [SerializeField]
    SaveSlot _firstSaveSlotPrefab;
    [SerializeField]
    SaveSlot _defaultSaveSlotPrefab;

    void Start()
    {
        RerenderSaveSlots();
        SaveSlotsHandler.OnSaveSlotsChanged += RerenderSaveSlots;
    }

    void RenderSaveSlot(SaveSlot prefabToCreate, int _index)
    {
        SaveSlot newSaveSlot = Instantiate(prefabToCreate, _saveSlotsContainerGO.transform);
        newSaveSlot.SetDATA(ProfilesHandler.SelectedProfile.SaveSlotsDATAs[_index]);
    }


    public void RerenderSaveSlots()
    {
        ClearAllSlots();

        RenderSaveSlot(_firstSaveSlotPrefab, 0);
        for (int i = 1; i < _saveSlots.Count; i++)
        {
            RenderSaveSlot(_defaultSaveSlotPrefab, i);
        }

    }

    void ClearAllSlots()
    {
        foreach (var item in _saveSlots)
        {
            Destroy(item.gameObject);
        }
        _saveSlots.Clear();
    }

    // public void OpenSaveSlotsWindow(SaveState _newSaveState)
    // {
    //     gameObject.SetActive(true);

    //     _currentSaveState = _newSaveState;
    // }

    public void OnCLoseWindow()
    {
        gameObject.SetActive(false);
    }
}
