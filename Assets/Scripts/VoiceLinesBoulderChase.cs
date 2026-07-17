using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesBoulderChase : MonoBehaviour
{
    AudioSource voiceSource;
    [SerializeField] AudioClip[] audioClips;
    AudioClip currentClip;
    AudioClip prevClip;
    [SerializeField] int minTime;
    [SerializeField] int maxTime;

    float waitTimeCountdown;
    public bool isNearby;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceSource = GetComponent<AudioSource>();
        prevClip = audioClips[0];
    }

    private void FixedUpdate()
    {
        if (!voiceSource.isPlaying)
        {
            if (isNearby)
            {
                if (waitTimeCountdown <= 0)
                {
                    currentClip = audioClips[Random.Range(0, audioClips.Length - 1)];
                    if (currentClip == prevClip) { currentClip = audioClips[Random.Range(0, audioClips.Length - 1)]; }
                    voiceSource.clip = currentClip;
                    voiceSource.Play();
                    waitTimeCountdown = Random.Range(minTime, maxTime);
                    prevClip = currentClip;
                }
                else
                {
                    waitTimeCountdown -= Time.fixedDeltaTime;
                }
            }
        }
    }
}
