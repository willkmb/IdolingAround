
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BoulderRoll : MonoBehaviour
{
    [SerializeField] VoiceLinesBoulderChase chaseVoicelines;
    [SerializeField] AudioSource voiceSource;
    
    [SerializeField] AudioClip audioClip;
    [HideInInspector] public Transform boulderRespawn;
    [HideInInspector] public float boulderRespawnTime;
    SplineAnimate splineAnim;
    Animation rollAnim;
    AudioSource rollSource;
    Rigidbody rb;

    float targetDistance;

    bool isEndOfCorridor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = GetComponent<SplineAnimate>();
        rb = GetComponent<Rigidbody>();
        rollAnim = GetComponentInChildren<Animation>();
        rollSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!isEndOfCorridor)
        {
            if (splineAnim.Duration - splineAnim.ElapsedTime < 0.1f)
            {
                chaseVoicelines.isNearby = false;
                voiceSource.clip = audioClip;
                voiceSource.Play();
                rollAnim.Stop();
                rollSource.loop = false;
                rollSource.Stop();
                isEndOfCorridor = true;
            }
        }

    }

    public void RespawnBoulder()
    {
        if (splineAnim.ElapsedTime != splineAnim.Duration)
        {
            splineAnim.Pause();
            splineAnim.ElapsedTime = boulderRespawnTime;
            splineAnim.Play();
        }
    }
}
