using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    MovementScript movementScript;
    DeathCounter deathCounter;
    Rigidbody rb;

    [Header("On Death")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] Animation transition1;
    [SerializeField] AudioSource respawnSound;

    [Header("Respawn Voicelines")]
    [SerializeField] TMP_Text checkpointSubtitles;
    [SerializeField] AudioClip[] voicelines;
    [SerializeField] string[] subtitles;

    [HideInInspector] public GameObject cutscene;
    [HideInInspector] public float cutsceneLength;
    [HideInInspector] public AudioSource voicePlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementScript = GetComponent<MovementScript>();
        deathCounter = GetComponent<DeathCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.R)) && !movementScript.isSpawning && movementScript.canRespawn)
        {
            StartSpawnOnKeyDown();
            Debug.Log("respawning");
        }
    }

    public void StartSpawn()
    {
        movementScript.isSpawning = true;
        movementScript.canRespawn = false;
        if (cutscene != null)
        {
            mainCamera.SetActive(false);
            cutscene.SetActive(true);
            transition1.Play();
            Invoke("Spawn", cutsceneLength);
            Invoke("RunDeathCounter", cutsceneLength);
            Invoke("PlayVoiceline", cutsceneLength);
        }
        else
        {
            Invoke("Spawn", 0.25f);
            Invoke("RunDeathCounter", 0.25f);
            Invoke("PlayVoiceline", 0.25f);
        }
    }

    public void StartSpawnOnKeyDown()
    {
        movementScript.isSpawning = true;
        movementScript.canRespawn = false;
        Spawn();
        RunDeathCounter();
        PlayVoiceline();
    }

    public void Spawn()
    {
        movementScript.StartCoroutine(movementScript.FreezeCube(1f));
        transition1.Play();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        this.transform.position = movementScript.respawnPoint.position;
        movementScript.respawnOnSide(movementScript.respawnPoint.eulerAngles.y);
        Debug.Log("should have moved");
        respawnSound.pitch = Random.Range(0.75f, 1.1f);
        respawnSound.Play();
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.Euler(0f, movementScript.respawnPoint.eulerAngles.y, 0f);
        Invoke("KinematicOff", 0.05f);
    }


    void RunDeathCounter()
    {
        deathCounter.UpdateDeathCounter();
    }
    void KinematicOff()
    {
        if (cutscene != null)
        {
            mainCamera.SetActive(true);
            cutscene.SetActive(false);
            cutscene = null;
        }
        rb.isKinematic = false;
        movementScript.isSpawning = false;
        Invoke("SetCanSpawn", 0.25f);

    }

    void PlayVoiceline()
    {
        int randomVL = Random.Range(0, voicelines.Length - 1);
        voicePlayer.clip = voicelines[randomVL];
        checkpointSubtitles.text = subtitles[randomVL];
        voicePlayer.Play();
        StartCoroutine(SetSubtitleEmpty());
    }

    
    IEnumerator SetSubtitleEmpty()
    {
        while (voicePlayer.isPlaying)
        {
            yield return new WaitForSeconds(0.05f);
        }
        checkpointSubtitles.text = "";
    }
    void SetCanSpawn()
    {
        movementScript.canRespawn = true;
    }
}
