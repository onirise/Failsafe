using UnityEngine;

namespace Failsafe.Player.Scripts
{
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
}
