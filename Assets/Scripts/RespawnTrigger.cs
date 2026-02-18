using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    MovementScript movementScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
    }

    private void OnTriggerEnter(Collider col)
    {
        col.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        col.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        col.gameObject.transform.position = movementScript.respawnPoint.position;
    }
}
