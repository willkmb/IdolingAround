using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class StartBoulder : MonoBehaviour
{
    Rigidbody playerRB;
    [SerializeField] MovementScript movementScript;
    [SerializeField] RespawnPlayer respawnPlayer;

    SplineAnimate splineAnim;

    [SerializeField] GameObject Boulder;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject cutscene;
    [SerializeField] Animation boulderAnim;
    [SerializeField] Animation transition1;
    [SerializeField] VoiceLinesBoulderStartRoll startRollVoicelines;
    [SerializeField] VoiceLinesBoulderChase chaseVoicelines;
    [SerializeField] BoulderRoll boulderRollScript;
    [SerializeField] AudioSource rollSound;

    bool hasStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //movementScript = FindFirstObjectByType<MovementScript>();
        //respawnPlayer = FindFirstObjectByType<RespawnPlayer>();
        playerRB = movementScript.gameObject.GetComponent<Rigidbody>();
        splineAnim = Boulder.GetComponent<SplineAnimate>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (!hasStarted)
        {
            mainCamera.SetActive(false);
            cutscene.SetActive(true);
            transition1.Play();
            movementScript.canSpeak = false;
            movementScript.timerRunning = false;
            movementScript.canRespawn = false;
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
        respawnPlayer.Spawn();
        movementScript.canSpeak = true;
        movementScript.timerRunning = true;
        movementScript.canRespawn = true;
        boulderRollScript.hasStartedRolling = true;
        cutscene.SetActive(false);
        mainCamera.SetActive(true);
        splineAnim.Play();
        boulderAnim.Play();
        chaseVoicelines.isNearby = true;
        rollSound.Play();
        yield return null;
    }
}
