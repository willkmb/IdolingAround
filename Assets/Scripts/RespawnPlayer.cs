using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    MovementScript movementScript;
    DeathCounter deathCounter;
    Rigidbody rb;

    [SerializeField] AudioSource respawnSound;

    [SerializeField] GameObject mainCamera;
    public GameObject cutscene;
    public float cutsceneLength;

    [SerializeField] Animation transition1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementScript = GetComponent<MovementScript>();
        deathCounter = GetComponent<DeathCounter>();
        respawnSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace)&& !movementScript.isSpawning && movementScript.canRespawn)
        {
            Invoke("StartSpawnOnKeyDown", 0f);
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
        }
        else
        {
            Invoke("Spawn", 0.25f);
        }
    }

    public void StartSpawnOnKeyDown()
    {
        movementScript.isSpawning = true;
        Invoke("Spawn", 0f);
    }

    void Spawn()
    {
        movementScript.StartCoroutine(movementScript.FreezeCube());
        transition1.Play();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        this.transform.position = movementScript.respawnPoint.position;
        this.transform.rotation = Quaternion.Euler(0f, movementScript.respawnPoint.eulerAngles.y, 0f);
        Debug.Log("should have moved");
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.Euler(0f, movementScript.respawnPoint.eulerAngles.y, 0f);
        respawnSound.Play();
        deathCounter.UpdateDeathCounter();
        Invoke("KinematicOff", 0.05f);
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
