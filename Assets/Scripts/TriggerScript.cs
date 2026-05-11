using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool inTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { inTrigger = true; }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { inTrigger = false; }
    }
}
