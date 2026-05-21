using UnityEngine;

public class RespawnTriggerBoulder : MonoBehaviour
{
    MovementScript movementScript;
    DeathCounter deathCounter;
    BoulderRoll boulderScript;
    AudioSource deathSound;
    GameObject Player;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = Player.GetComponent<Rigidbody>();
        movementScript = Player.GetComponent<MovementScript>();
        deathCounter = Player.GetComponent<DeathCounter>();
        boulderScript = GetComponentInParent<BoulderRoll>();
        deathSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (movementScript.isSpawning == false)
        {
            if (col.gameObject.layer == 6)
            {
                movementScript.isSpawning = true;
                deathSound.Play();
                Invoke("Spawn", 0.5f);
            }
        }
    }

    void Spawn()
    {
        boulderScript.RespawnBoulder();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        Player.transform.position = movementScript.respawnPoint.position;
        Debug.Log("should have moved");
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.identity;
        deathCounter.UpdateDeathCounter();
        Invoke("KinematicOff", 0.05f);
    }

    void KinematicOff()
    {
        rb.isKinematic = false;
        movementScript.isSpawning = false;
    }


}
