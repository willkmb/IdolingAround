using UnityEngine;

public class PotSmash : MonoBehaviour
{
    [SerializeField] AudioSource sound;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;

    private void Awake()
    {
        sound.pitch = Random.Range(minPitch, maxPitch);
        sound.Play();
    }

}
