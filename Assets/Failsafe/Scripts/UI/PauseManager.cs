using UnityEngine;

public class PauseManager 
{
    static bool isPause = false;

    public static void Pause(bool flag)
    {
        isPause = flag;
    }

    public static bool CheckPause()
    {
        return isPause;
    }
}
