using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;


public class GameplaySave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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


    bool _isStartAutosave = false;



    public void SetDATA(GameplaySaveDATA _gSaveDATA, bool _setStartAutosave = false)
    {
        _isStartAutosave = _setStartAutosave;
        if (_isStartAutosave)
            _clearButtonGO.SetActive(false);

        _lastSaveTextGO.SetActive(_gSaveDATA.LastSave);
        _timeLocalizeStringEvent.StringReference.Arguments = new object[] { TimeSpan.FromSeconds(_gSaveDATA.Time) };
        _timeLocalizeStringEvent.RefreshString();
        SetScreenshot(_gSaveDATA);
        //DATA.IsEmpty = _isEmpty;

    }

    void SetScreenshot(GameplaySaveDATA _gSaveDATA)
    {
        if (_gSaveDATA.ScreenshotLink != "")
        {
            byte[] fileData = File.ReadAllBytes(_gSaveDATA.ScreenshotLink);
            Texture2D loadedTexture = new Texture2D(2, 2); // Временные размеры (автоматически изменятся)
            loadedTexture.LoadImage(fileData);
            _savePreview.texture = loadedTexture;
        }
        else
        {
            _savePreview.texture = null;
        }
    }


    public void ClearSave()
    {
        //SetDATA(_gSaveDATA); // ну наверное так оно будет
        SaveManager.SaveAll();



    }


    public void SetSaveName(string _entryName, int id)
    {
        _nameTextLocEvent.SetEntry(_entryName);
        if (id != -1)
            _nameTextLocEvent.StringReference.Arguments = new object[] { id };
        _nameTextLocEvent.RefreshString();
    }


    public void DeselectGameplaySave()
    {
        //DATA.LastSave = false;
        _lastSaveTextGO.SetActive(false);

    }

    public void OnSaveGameplaySave()
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

    public void OnLoadGameplaySave()
    {
        // if (!DATA.IsEmpty)
        // {
        //     ClickToSelectTextGO.SetActive(false);
        //     //gameplaySavesHandler.profileParent.SelectCLickedProfile();
        //     _gameplaySavesHandler.GameplaySavesGO.SetActive(false);
        //     SaveManager.SaveAll();
        // }

    }



    public void OnMouseEnterToGameplaySave()
    {
        // if ((!_isStartAutosave && _gameplaySavesHandler.CurrentSaveState == SaveState.Save) || (!DATA.IsEmpty && _gameplaySavesHandler.CurrentSaveState == SaveState.Load))
        // {
        //     ClickToSelectTextGO.SetActive(true);
        // }




    }

    public void OnMouseExitToGameplaySave()
    {

        _clickToSelectTextGO.SetActive(false);



    }


    // Использовал ивенты таким образом, ибо если использовать компонент Event trigger,
    // то не будет работать скроллинг профилей пока мышка находится на одном из профилей
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterToGameplaySave();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitToGameplaySave();
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
