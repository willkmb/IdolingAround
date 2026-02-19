using System.Collections;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    MovementScript movementScript;
    AudioSource sound;
    Collider coll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        coll = col;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.5f);
        coll.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        coll.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        coll.gameObject.transform.position = movementScript.respawnPoint.position;
        sound.Play();
    }
}
