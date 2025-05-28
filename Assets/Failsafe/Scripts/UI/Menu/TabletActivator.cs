using UnityEngine;

public class TabletActivator : MonoBehaviour
{
    [SerializeField]
    GameObject _tabletGO;


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
        SetCursor();
        PauseManager.Pause(_tabletGO.activeSelf);
        //gameplaySavesHandler.gameplaySavesGO.SetActive(false);
    }

    void SetCursor()
    {
        Cursor.visible = _tabletGO.activeSelf;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

}
