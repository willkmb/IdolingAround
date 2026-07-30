
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


    private void Update()
    {
        if (!isEndOfCorridor)
        {
            if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.R)) && trig.hasEntered)
            {
                Debug.Log("Respawning Boulder");

                splineAnim.Pause();
                if (splineAnim.ElapsedTime > boulderRespawnTime) { splineAnim.ElapsedTime = boulderRespawnTime; }            
                splineAnim.Play();
            }

            if (splineAnim.Duration - splineAnim.ElapsedTime < 0.1f)
            {
                chaseVoicelines.isNearby = false;
                rollAnim.Stop();
                rollSource.loop = false;
                rollSource.Stop();
                rollSource.clip = audioClip;
                rollSource.pitch = 1;
                rollSource.volume = 0.4f;
                rollSource.Play();
                boulderKillTrigger.SetActive(false);
                boulderInvisWall.SetActive(true);
                isEndOfCorridor = true;
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
}
