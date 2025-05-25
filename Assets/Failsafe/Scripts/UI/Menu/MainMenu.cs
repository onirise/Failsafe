using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    [SerializeField]
    Button playButton;

    public GameObject profilesMainGO;

    [Inject] ProfilesHandler profilesHandler;

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
        profilesMainGO.SetActive(true);
    }

    private void OnEnable()
    {
        CheckSelectedProfile();
    }

    void CheckSelectedProfile()
    {
        playButton.interactable = profilesHandler.profiles1.Count > 0;
    }




}
