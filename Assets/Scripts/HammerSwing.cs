using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class HammerSwing : MonoBehaviour
{
    [SerializeField] AudioSource sound;
    bool soundPlayed;

    private void Start()
    {
        Invoke("PlaySound", 0.5f);
    }

    void PlaySound()
    {
        sound.Play();
    }
}
