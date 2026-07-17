using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    MovementScript movementScript;
    DeathCounter deathCounter;
    Rigidbody rb;
    
    [SerializeField] GameObject mainCamera;
    [SerializeField] Animation transition1;
    [SerializeField] AudioSource respawnSound;

    [HideInInspector] public GameObject cutscene;
    [HideInInspector] public float cutsceneLength;


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
        if (Input.GetKeyDown(KeyCode.Backspace) && !movementScript.isSpawning && movementScript.canRespawn)
        {
            StartSpawnOnKeyDown();
            Debug.Log("respawning");
        }
    }

    public void StartSpawn()
    {
        movementScript.isSpawning = true;
        if (cutscene != null)
        {
            mainCamera.SetActive(false);
            cutscene.SetActive(true);
            transition1.Play();
            Invoke("Spawn", cutsceneLength);
            Invoke("RunDeathCounter", cutsceneLength);
        }
        else
        {
            Invoke("Spawn", 0.25f);
            Invoke("RunDeathCounter", 0.25f);
        }
    }

    public void StartSpawnOnKeyDown()
    {
        movementScript.isSpawning = true;
        Spawn();
        RunDeathCounter();
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
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.Euler(0f, movementScript.respawnPoint.eulerAngles.y, 0f);
        respawnSound.pitch = Random.Range(0.75f, 1.1f);
        respawnSound.Play();
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

    }
}
