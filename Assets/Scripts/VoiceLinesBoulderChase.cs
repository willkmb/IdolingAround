using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesBoulderChase : MonoBehaviour
{
    AudioSource voiceSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] int minTime;
    [SerializeField] int maxTime;

    float waitTimeCountdown;
    public bool isNearby;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!voiceSource.isPlaying)
        {
            if (isNearby)
            {
                if (waitTimeCountdown <= 0)
                {
                    voiceSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
                    voiceSource.Play();
                    waitTimeCountdown = Random.Range(minTime, maxTime);
                }
                else
                {
                    waitTimeCountdown -= Time.fixedDeltaTime;
                }
            }
        }
    }
}
