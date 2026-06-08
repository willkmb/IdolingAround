using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    MovementScript movementScript;
    AudioSource sound;
    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystem particleRock;
    [SerializeField] Transform respawnPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
        particleRock = GetComponentInChildren<ParticleSystem>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            respawnPoint.gameObject.transform.position = other.gameObject.transform.position;
            movementScript.respawnPoint = respawnPoint;
            sound.Play();
            //particle.gameObject.transform.position = other.gameObject.transform.position;
            particle = GetComponentInChildren<ParticleSystem>();
            particle.Play();
            //Invoke("ParticleOff", 1f);
        }
    }

    void ParticleOff()
    {
        //particle.SetActive(false);
    }
}
