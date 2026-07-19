using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleRock;
    [SerializeField] private Transform respawnPoint;
    public Animation totem;
    [SerializeField] GameObject totemPole;
    [SerializeField] Material totemMat;
    [SerializeField] Material totemMatGlow;

    private MovementScript movementScript;
    private AudioSource sound;
    private bool triggered = false;

    void Start()
    {
        movementScript = FindFirstObjectByType<MovementScript>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        totemPole.GetComponentInChildren<MeshRenderer>().material = totemMatGlow;

        if (other.gameObject.layer != 6) return;
        if (triggered) return;

        movementScript.respawnPoint = respawnPoint;
        totem.Play();
        totem.gameObject.GetComponentInChildren<AudioSource>().Play();
        particleRock.Play();
        triggered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 6) return;
        totemPole.GetComponentInChildren<MeshRenderer>().material = totemMat;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 6) return;

        //replace with coroutine to prevent snapping when player enters trigger
        totemPole.transform.LookAt(new Vector3(other.transform.position.x, totemPole.transform.position.y, other.transform.position.z));
    }
}