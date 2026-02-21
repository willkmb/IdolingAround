using System.Collections;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    MovementScript movementScript;
    AudioSource sound;
    [SerializeField] AudioSource DeathSound;
    Collider coll;
    GameObject Player;
    Rigidbody rb;
    //bool isSpawning;
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
                coll = col;
                //rb.isKinematic = true;
                Invoke("Spawn", 0.5f);
            }
        }


    }


    void Spawn()
    {
        DeathSound.Play();
        rb.isKinematic= true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //coll.gameObject.transform.position = movementScript.respawnPoint.position;
        //coll.gameObject.transform.position = coll.gameObject.GetComponent<MovementScript>().respawnPoint.position;
        Player.transform.position = movementScript.respawnPoint.position;
        //rb.isKinematic = false;
        Debug.Log("should have moved");
        //movementScript.gameObject.transform.Find("CameraTarget").transform.rotation = Quaternion.identity;
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.identity;
        sound.Play();
        //movementScript.isSpawning = false;
        //rb.isKinematic = false;
        Invoke("KinematicOff", 0.05f);
    }

    void KinematicOff()
    {
        rb.isKinematic = false;
        movementScript.isSpawning = false;
    }


}
