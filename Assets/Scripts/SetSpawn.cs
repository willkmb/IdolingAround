using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    MovementScript movementScript;
    AudioSource sound;
    [SerializeField] ParticleSystem particle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
        particle = GetComponentInChildren<ParticleSystem>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            movementScript.respawnPoint = this.gameObject.transform;
            sound.Play();
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
