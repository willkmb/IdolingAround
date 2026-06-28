using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class StartBoulder : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] MovementScript movementScript;
    SplineAnimate splineAnim;
    [SerializeField] GameObject Boulder;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject cutscene;
    [SerializeField] Animation boulderAnim;
    [SerializeField] Animation transition1;
    [SerializeField] VoiceLinesBoulderStartRoll startRollVoicelines;
    [SerializeField] VoiceLinesBoulderChase chaseVoicelines;
    [SerializeField] AudioSource rollSound;
    [SerializeField] ghostRecorder ghost;
    bool hasStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = Boulder.GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasStarted)
        {
            mainCamera.SetActive(false);
            cutscene.SetActive(true);
            transition1.Play();
            movementScript.timerRunning = false;
            movementScript.canRespawn = false;
            ghost.PauseRecording();
            Invoke("PlayerKinematic", 0.5f);
            hasStarted = true;
        }
    }

    void PlayerKinematic()
    {
        playerRB.isKinematic = true;
    }

    public IEnumerator StartBoulderRoll()
    {
        playerRB.isKinematic = false;
        movementScript.timerRunning = true;
        movementScript.canRespawn = true;
        transition1.Play();
        cutscene.SetActive(false);
        mainCamera.SetActive(true);
        splineAnim.Play();
        boulderAnim.Play();
        chaseVoicelines.isNearby = true;
        rollSound.Play();
        ghost.ResumeRecording();
        yield return null;
    }
}
