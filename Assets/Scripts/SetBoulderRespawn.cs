using UnityEngine;

public class SetBoulderRespawn : MonoBehaviour
{
    [SerializeField] BoulderRoll boulderScript;
    [SerializeField] Transform boulderRespawn;

    private void OnTriggerEnter(Collider other)
    {
        boulderScript.boulderRespawn = boulderRespawn;
    }
}
