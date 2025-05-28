using UnityEngine;

public class PauseManager
{
    static bool _isPause = false;

    public static void Pause(bool flag)
    {
        _isPause = flag;
    }

    public static bool CheckPause()
    {
        return _isPause;
    }
}
