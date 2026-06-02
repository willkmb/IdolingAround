using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SetSpawnBoulder : MonoBehaviour
{
    MovementScript movementScript;
    AudioSource sound;
    [SerializeField] ParticleSystem particle;
    [SerializeField] BoulderRoll boulderScript;
    [SerializeField] Transform respawnPoint;
    //[SerializeField] int knotNumber;
    [SerializeField] int boulderTime;


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
            movementScript.respawnPoint = respawnPoint;
            sound.Play();
            particle.gameObject.transform.position = other.gameObject.transform.position;
            particle = GetComponentInChildren<ParticleSystem>();
            particle.Play();
            //Invoke("ParticleOff", 1f);
            boulderScript.boulderRespawnTime = boulderTime;

        }
    }

    void ParticleOff()
    {
        //particle.SetActive(false);
    }
}
