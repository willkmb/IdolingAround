using System.Collections;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    MovementScript movementScript;
    AudioSource sound;
    [SerializeField] AudioSource DeathSound;
    Collider coll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6)
        {
            coll = col;
            Invoke("Spawn", 0.5f);
        }

    }


    void Spawn()
    {
        DeathSound.Play();
        coll.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        coll.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        coll.gameObject.transform.position = movementScript.respawnPoint.position;
        //movementScript.gameObject.transform.Find("CameraTarget").transform.rotation = Quaternion.identity;
        GameObject.Find("CameraTarget").transform.rotation = Quaternion.identity;
        sound.Play();
    }


}
