using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;


public class GameplaySave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameplaySaveDATA DATA;


    [SerializeField]
    LocalizeStringEvent _nameTextLocEvent;
    [SerializeField]
    LocalizeStringEvent _timeLocalizeStringEvent;
    [SerializeField]
    GameObject _clickToSelectTextGO;
    [SerializeField]
    GameObject _selectedTextGO;
    [SerializeField]
    GameObject _clearButtonGO;
    [SerializeField]
    RawImage _savePreview;


    bool _isStartAutosave = false;


    //[Inject] GameplaySavesHandler _gameplaySavesHandler;

    public void UpdateGameplaySaveUI(bool _setStartAutosave = false)
    {
        _isStartAutosave = _setStartAutosave;
        if (_isStartAutosave)
            _clearButtonGO.SetActive(false);

        _selectedTextGO.SetActive(DATA.LastSave);
        TimeSpan timeSpan = TimeSpan.FromSeconds(DATA.Time); //tabletHandler.time
        string timeFormatted = timeSpan.ToString(@"hh\:mm\:ss");
        _timeLocalizeStringEvent.StringReference.Arguments = new object[] { timeFormatted };
        _timeLocalizeStringEvent.RefreshString();

        //временно взял этот код вообще из другого места
        if (DATA.ScreenshotLink != "")
        {
            byte[] fileData = File.ReadAllBytes(DATA.ScreenshotLink);
            Texture2D loadedTexture = new Texture2D(2, 2); // Временные размеры (автоматически изменятся)
            loadedTexture.LoadImage(fileData);
            _savePreview.texture = loadedTexture;
        }
        else
        {
            _savePreview.texture = null;
        }
    }



    public void SetNewDATA(string _scrLink, bool _lastSave, float _time, bool _isEmpty)
    {
        DATA.ScreenshotLink = _scrLink;
        DATA.LastSave = _lastSave;
        DATA.Time = _time;
        DATA.IsEmpty = _isEmpty;
    }

    public void ClearDATA()
    {
        SetNewDATA("", false, 0, true);
        UpdateGameplaySaveUI();
        SaveManager.SaveAll();
        // for (int i = _gameplaySavesHandler.GameplaySaves.Count - 1; i >= 0; i--)
        // {
        //     if (!_gameplaySavesHandler.GameplaySaves[i].DATA.IsEmpty)
        //     {
        //         _gameplaySavesHandler.GameplaySaves[i].DATA.LastSave = true;
        //         _gameplaySavesHandler.GameplaySaves[i].SelectedTextGO.SetActive(true);
        //         break;
        //     }


        // }


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
        DATA.LastSave = false;
        _selectedTextGO.SetActive(false);

    }

    public void OnSaveGameplaySave()
    {

        // foreach (var item in _gameplaySavesHandler.GameplaySaves)
        // {
        //     item.DeselectGameplaySave();
        // }


        //string link = screenTaker.SaveCameraView(this);
        //SetNewDATA(link, true, tabletHandler.time, false);
        UpdateGameplaySaveUI();
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
