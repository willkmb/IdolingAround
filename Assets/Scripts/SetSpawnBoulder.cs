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

    [SerializeField] private ParticleSystem particleRock;
    public Animation totem;
    private bool triggered = false;

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
            if (triggered) return;

            movementScript.respawnPoint = respawnPoint;
            sound.Play();
            //particle.gameObject.transform.position = other.gameObject.transform.position;
            //particle = GetComponentInChildren<ParticleSystem>();
            //particle.Play();
            totem.Play();
            totem.gameObject.GetComponent<AudioSource>().Play();
            particleRock.Play();
            //Invoke("ParticleOff", 1f);
            boulderScript.boulderRespawnTime = boulderTime;
            triggered = true;

        }
    }

    void ParticleOff()
    {
        //particle.SetActive(false);
    }
}
