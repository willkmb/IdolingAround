using UnityEngine;

public class GhostPauser : MonoBehaviour
{
    public static bool paused = false;

    private void Awake()
    {
        paused = true;
    }

    public static void PauseGhosts()
    {
        paused = true;
    }

    public static void ResumeGhosts()
    {
        paused = false;
    }
}
