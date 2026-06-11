using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleRock;
    [SerializeField] private Transform respawnPoint;
    public Animation totem;

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
        if (other.gameObject.layer != 6) return;
        if (triggered) return;

        //respawnPoint.position = other.transform.position;
        movementScript.respawnPoint = respawnPoint;

        sound.Play();
        totem.Play();
        totem.gameObject.GetComponent<AudioSource>().Play();
        particleRock.Play();
        triggered = true;
    }
}