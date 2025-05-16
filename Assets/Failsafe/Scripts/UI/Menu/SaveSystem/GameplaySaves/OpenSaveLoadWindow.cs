using UnityEngine;

public class OpenSaveLoadWindow : MonoBehaviour
{
    public GameObject windowToOpen;

    public void OpenWindow()
    {
        windowToOpen.SetActive(true);
    }
}
