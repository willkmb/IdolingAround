using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesBoulderChase : MonoBehaviour
{
    AudioSource voiceSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] TMP_Text boulderSubtitle;
    [SerializeField] string[] audioSubtitles;
    AudioClip currentClip;
    AudioClip prevClip;
    [SerializeField] int minTime;
    [SerializeField] int maxTime;

    float waitTimeCountdown;
    public bool isNearby;
    PauseMenu pause;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceSource = GetComponent<AudioSource>();
        prevClip = audioClips[0];
        pause = FindFirstObjectByType<PauseMenu>();
    }

    private void FixedUpdate()
    {

        if (!voiceSource.isPlaying && !pause.paused)
        {
            if (isNearby)
            {
                if (waitTimeCountdown <= 0)
                {
                    var clipInt = Random.Range(0, audioClips.Length - 1);
                    currentClip = audioClips[clipInt];
                    if (currentClip == prevClip) { clipInt = Random.Range(0, audioClips.Length - 1); currentClip = audioClips[clipInt]; }
                    voiceSource.clip = currentClip;
                    boulderSubtitle.text = audioSubtitles[clipInt];
                    voiceSource.Play();
                    SetSubtitle(audioSubtitles[clipInt]);
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

    void SetSubtitle(string subtitle)
    {
        boulderSubtitle.text = subtitle;
        Invoke("BlankSubtitle", currentClip.length);
    }

    void BlankSubtitle()
    {
        boulderSubtitle.text = "";
    }
}
