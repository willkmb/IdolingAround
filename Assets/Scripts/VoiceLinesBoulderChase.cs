using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesBoulderChase : MonoBehaviour
{
    AudioSource voiceSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] int minTime;
    [SerializeField] int maxTime;

    public bool isNearby;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceSource = GetComponent<AudioSource>();
    }

    public IEnumerator Talk()
    {
        while (isNearby)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            voiceSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
        }
        yield return null;
    }
}
