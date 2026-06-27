
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BoulderRoll : MonoBehaviour
{
    [SerializeField] VoiceLinesBoulderChase chaseVoicelines;
    [SerializeField] AudioSource voiceSource;
    
    [SerializeField] AudioClip audioClip;
    [SerializeField] GameObject boulderKillTrigger;
    [SerializeField] GameObject boulderInvisWall;
    [HideInInspector] public Transform boulderRespawn;
    [HideInInspector] public float boulderRespawnTime;
    [SerializeField] StartBoulderTrigger trig;
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
                boulderKillTrigger.SetActive(false);
                boulderInvisWall.SetActive(true);
                isEndOfCorridor = true;
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && trig.hasEntered)
        {
            Debug.Log("Respawning Boulder");

            splineAnim.Pause();
            if (splineAnim.ElapsedTime > boulderRespawnTime) { splineAnim.ElapsedTime = boulderRespawnTime; }            
            splineAnim.Play();
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
