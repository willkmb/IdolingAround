using UnityEngine;

public class ExhibitionVerToggle : MonoBehaviour
{
    public bool ExhibitionMode = false;
    [SerializeField] ghostPlay ghost;

    private void Start()
    {
        ghost.playbackSpeed = ExhibitionMode ? 1.005f : 1f;
    }

}
