using UnityEngine;

public class TabletActivator : MonoBehaviour
{
    [SerializeField]
    GameObject _tabletGO;

    void Start()
    {
        EnableTablet(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            EnableTablet(!_tabletGO.activeSelf);

        }
    }

    public void EnableTablet(bool flag)
    {
        _tabletGO.SetActive(flag);
        SetCursor(flag);
        PauseManager.Pause(flag);
        //gameplaySavesHandler.gameplaySavesGO.SetActive(false);
    }

    void SetCursor(bool flag)
    {
        Cursor.visible = flag;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

}
