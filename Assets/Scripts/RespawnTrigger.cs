using System.Collections;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    MovementScript movementScript;
    AudioSource sound;
    [SerializeField] AudioSource DeathSound;
    GameObject Player;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("IdolCapsule");
        rb = Player.GetComponent<Rigidbody>();
        movementScript = Player.GetComponent<MovementScript>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (movementScript.isSpawning == false)
        {
            if (col.gameObject.layer == 6)
            {
                movementScript.isSpawning = true;
                DeathSound.Play();
                Invoke("Spawn", 0.5f);
            }
        }
    }

    void Spawn()
    {
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Player.transform.position = movementScript.respawnPoint.position;
        Debug.Log("should have moved");
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.identity;
        sound.Play();
        movementScript.deaths++;
        Invoke("KinematicOff", 0.05f);
    }

    void KinematicOff()
    {
        rb.isKinematic = false;
        movementScript.isSpawning = false;
    }


}
