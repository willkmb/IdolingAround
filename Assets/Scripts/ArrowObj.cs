using System;
using UnityEngine;

public class ArrowObj : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("FiresArrow")) return;
        if (other.isTrigger) return;
        gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
