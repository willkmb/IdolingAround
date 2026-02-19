using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    MovementScript movementScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            movementScript.respawnPoint = this.gameObject.transform;
        }
    }
}
