
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BoulderRoll : MonoBehaviour
{
    [Header("Audio and Voicelines")]
    [SerializeField] VoiceLinesBoulderChase chaseVoicelines;
    AudioSource rollSource;

    [SerializeField] AudioClip audioClip;
    [SerializeField] string audioSubtitle;
    [SerializeField] TMP_Text boulderSubtitles;

    [Header("End of Corridor Objects")]
    [SerializeField] GameObject boulderKillTrigger;
    [SerializeField] GameObject boulderInvisWall;

    [HideInInspector] public Transform boulderRespawn;
    [HideInInspector] public float boulderRespawnTime;

    [Header("Trigger")]
    [SerializeField] StartBoulderTrigger trig;
    
    SplineAnimate splineAnim;
    Animation rollAnim;

    PauseMenu pause;

    bool isEndOfCorridor;
    public bool hasStartedRolling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = GetComponent<SplineAnimate>();
        rollAnim = GetComponentInChildren<Animation>();
        rollSource = GetComponent<AudioSource>();
        pause = FindFirstObjectByType<PauseMenu>();
    }


    private void Update()
    {
        if (!isEndOfCorridor)
        {
            if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.R)) && trig.hasEntered && !pause.paused)
            {
                Debug.Log("Respawning Boulder");

                splineAnim.Pause();
                if (splineAnim.ElapsedTime > boulderRespawnTime) { splineAnim.ElapsedTime = boulderRespawnTime; }            
                splineAnim.Play();
            }

            if (splineAnim.Duration - splineAnim.ElapsedTime < 0.1f && !pause.paused)
            {
                chaseVoicelines.isNearby = false;
                rollAnim.Stop();
                rollSource.loop = false;
                rollSource.Stop();
                rollSource.clip = audioClip;
                rollSource.pitch = 1;
                rollSource.volume = 0.4f;
                rollSource.Play();
                SetSubtitles();
                boulderKillTrigger.SetActive(false);
                boulderInvisWall.SetActive(true);
                isEndOfCorridor = true;
            }

            if (hasStartedRolling)
            {
                if (pause.paused) 
                { 
                    splineAnim.Pause();
                    rollSource.Pause();
                    rollAnim["BoulderRoll"].speed = 0f;
                }
                else 
                { 
                    splineAnim.Play();
                    rollSource.UnPause();
                    rollAnim["BoulderRoll"].speed = 1f;
                }
            }

        }


    }

    public void RespawnBoulder()
    {
        if (!isEndOfCorridor)
        {
            splineAnim.Pause();
            splineAnim.ElapsedTime = boulderRespawnTime;
            splineAnim.Play();
        }
    }

    void SetSubtitles()
    {
        boulderSubtitles.text = audioSubtitle;
        Invoke("BlankSubtitles", audioClip.length);
    }
    void BlankSubtitles()
    {
        boulderSubtitles.text = "";
    }
}
