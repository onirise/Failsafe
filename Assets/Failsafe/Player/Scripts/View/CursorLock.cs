using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorLock : MonoBehaviour
{
    [SerializeField] private bool _lockCursor = true;


    private void Start()
    {
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
