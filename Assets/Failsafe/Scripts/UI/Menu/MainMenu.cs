using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;

public class MainMenu : BaseMenu
{
    [SerializeField]
    Button _playButton;
    [SerializeField]
    GameObject _profilesMainGO;



    void Start()
    {

        CheckSelectedProfile();
    }

    public void ExitGame()
    {
        CallConfirm(() =>
        {
            Debug.LogWarning("Quit");
            Application.Quit();
        });

    }

    public void OnProfilesOpen()
    {
        gameObject.SetActive(false);
        _profilesMainGO.SetActive(true);
    }

    private void OnEnable()
    {
        CheckSelectedProfile();
    }

    void CheckSelectedProfile()
    {
        _playButton.interactable = ProfilesHandler.IsProfilesGreaterThanZero();
    }




}
