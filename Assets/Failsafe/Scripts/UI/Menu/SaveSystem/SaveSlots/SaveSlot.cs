using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;


public class SaveSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    LocalizeStringEvent _nameTextLocEvent;
    [SerializeField]
    LocalizeStringEvent _timeLocalizeStringEvent;
    [SerializeField]
    GameObject _clickToSelectTextGO;
    [SerializeField]
    GameObject _lastSaveTextGO;
    [SerializeField]
    GameObject _clearButtonGO;
    [SerializeField]
    RawImage _savePreview;


    //bool _isStartAutosave = false;



    public void SetDATA(SaveSlotDATA _saveSlotData) // bool _setStartAutosave = false
    {
        // _isStartAutosave = _setStartAutosave;
        // if (_isStartAutosave)
        //     _clearButtonGO.SetActive(false);

        _lastSaveTextGO.SetActive(_saveSlotData.LastSave);
        _timeLocalizeStringEvent.StringReference.Arguments = new object[] { TimeSpan.FromSeconds(_saveSlotData.Time) };
        _timeLocalizeStringEvent.RefreshString();
        SetScreenshot(_saveSlotData);
        //DATA.IsEmpty = _isEmpty;

    }

    void SetScreenshot(SaveSlotDATA _saveSlotData)
    {
        if (_saveSlotData.ScreenshotLink != "")
        {
            byte[] fileData = File.ReadAllBytes(_saveSlotData.ScreenshotLink);
            Texture2D loadedTexture = new Texture2D(2, 2);
            loadedTexture.LoadImage(fileData);
            _savePreview.texture = loadedTexture;
        }
        else
        {
            _savePreview.texture = null;
        }
    }


    public void ClearSaveSlot()
    {
        //SetDATA(_gSaveDATA); // ну наверное так оно будет
        SaveManager.SaveAll();



    }


    // public void SetSaveName(string _entryName, int id)
    // {
    //     _nameTextLocEvent.SetEntry(_entryName);
    //     if (id != -1)
    //         _nameTextLocEvent.StringReference.Arguments = new object[] { id };
    //     _nameTextLocEvent.RefreshString();
    // }


    public void DeselectSaveSlot()
    {
        //DATA.LastSave = false;
        _lastSaveTextGO.SetActive(false);

    }

    public void OnSaveSaveSlot()
    {

        // foreach (var item in _gameplaySavesHandler.GameplaySaves)
        // {
        //     item.DeselectGameplaySave();
        // }


        //string link = screenTaker.SaveCameraView(this);

        //SetDATA();
        _clickToSelectTextGO.SetActive(false);
        //gameplaySavesHandler.profileParent.SelectCLickedProfile();
        SaveManager.SaveAll();
        //gameplaySavesHandler.gameplaySavesGO.SetActive(false);



    }

    public void OnLoadSaveSlot()
    {
        // if (!DATA.IsEmpty)
        // {
        //     ClickToSelectTextGO.SetActive(false);
        //     //gameplaySavesHandler.profileParent.SelectCLickedProfile();
        //     _gameplaySavesHandler.GameplaySavesGO.SetActive(false);
        //     SaveManager.SaveAll();
        // }

    }



    public void OnMouseEnterToSaveSlot()
    {
        // if ((!_isStartAutosave && _gameplaySavesHandler.CurrentSaveState == SaveState.Save) || (!DATA.IsEmpty && _gameplaySavesHandler.CurrentSaveState == SaveState.Load))
        // {
        //     ClickToSelectTextGO.SetActive(true);
        // }




    }

    public void OnMouseExitFromSaveSlot()
    {

        _clickToSelectTextGO.SetActive(false);



    }


    // Использовал ивенты таким образом, ибо если использовать компонент Event trigger,
    // то не будет работать скроллинг профилей пока мышка находится на одном из профилей
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterToSaveSlot();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitFromSaveSlot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // if (_gameplaySavesHandler.CurrentSaveState == SaveState.Save && !_isStartAutosave)
        // {
        //     OnSaveGameplaySave();
        // }
        // else
        //     OnLoadGameplaySave();


    }

}
