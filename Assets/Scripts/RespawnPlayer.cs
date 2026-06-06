using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    MovementScript movementScript;
    DeathCounter deathCounter;
    Rigidbody rb;

    [SerializeField] AudioSource respawnSound;

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
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Invoke("StartSpawnOnKeyDown", 0f);
            Debug.Log("respawning");
        }
    }

    public void StartSpawn()
    {
        movementScript.isSpawning = true;
        Invoke("Spawn", 0.5f);
    }

    public void StartSpawnOnKeyDown()
    {
        movementScript.isSpawning = true;
        Invoke("Spawn", 0f);
    }

    void Spawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        this.transform.position = movementScript.respawnPoint.position;
        Debug.Log("should have moved");
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.identity;
        respawnSound.Play();
        deathCounter.UpdateDeathCounter();
        Invoke("KinematicOff", 0.05f);
    }

    void KinematicOff()
    {
        rb.isKinematic = false;
        movementScript.isSpawning = false;
    }
}
