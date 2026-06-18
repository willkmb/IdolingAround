using Unity.VisualScripting;
using UnityEngine;

public class StartBoulderTrigger : MonoBehaviour
{
    public bool hasEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            hasEntered = true;
        }
    }
}
