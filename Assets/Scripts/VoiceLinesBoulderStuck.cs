using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesBoulderStuck : MonoBehaviour
{
    AudioSource voiceSource;
    [SerializeField] TMP_Text boulderSubtitles;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] string[] audioSubtitles;
    [SerializeField] int minTime;
    [SerializeField] int maxTime;

    float waitTimeCountdown;
    public bool isNearby = false;

    int currentVL;
    int prevVL = 1;
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
                    StartCoroutine(Voiceline());
                    waitTimeCountdown = Random.Range(minTime, maxTime);
                }
                else
                {
                    waitTimeCountdown -= Time.fixedDeltaTime;
                }
            }
        }
    }

    IEnumerator Voiceline()
    {
        int value = Random.Range(0, audioClips.Length - 1);
        currentVL = value;
        if (currentVL == prevVL) { value = Random.Range(0, audioClips.Length - 1); currentVL = value; }
        
        voiceSource.clip = audioClips[value];
        voiceSource.Play();

        boulderSubtitles.text = audioSubtitles[value];

        prevVL = currentVL;
        yield return new WaitForSeconds(voiceSource.clip.length);
        boulderSubtitles.text = "";


        yield return null;
    }
}
