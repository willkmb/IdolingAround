using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesBoulderStuck : MonoBehaviour
{
    AudioSource voiceSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] int minTime;
    [SerializeField] int maxTime;

    float waitTimeCountdown;
    public bool isNearby = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNearby = false;
        }
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
