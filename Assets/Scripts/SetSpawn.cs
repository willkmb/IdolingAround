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
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            movementScript.respawnPoint = this.gameObject.transform;
            sound.Play();
            particle.Play();
            //Invoke("ParticleOff", 1f);
        }
    }

    void ParticleOff()
    {
        //particle.SetActive(false);
    }
}
