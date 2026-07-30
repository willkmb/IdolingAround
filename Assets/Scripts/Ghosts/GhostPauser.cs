using UnityEngine;

public class GhostPauser : MonoBehaviour
{
    public static bool paused = false;

    public static void PauseGhosts()
    {
        paused = true;
    }

    public static void ResumeGhosts()
    {
        paused = false;
    }
}
