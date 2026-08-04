using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SetSpawnBoulder : MonoBehaviour
{
    MovementScript movementScript;
    RespawnPlayer respawnPlayer;
    AudioSource sound;

    [Header("Boulder and Respawn")]
    [SerializeField] BoulderRoll boulderScript;
    [SerializeField] int boulderTime;
    [SerializeField] Transform respawnPoint;

    [Header("Totem")]
    [SerializeField] private ParticleSystem particleRock;
    public Animation totem;
    [SerializeField] GameObject totemPole;
    [SerializeField] Material totemMat;
    [SerializeField] Material totemMatGlow;

    [Header("Audio")]
    [SerializeField] AudioSource voiceSource;
    private bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>();
        respawnPlayer = GameObject.FindFirstObjectByType<RespawnPlayer>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            totemPole.GetComponentInChildren<MeshRenderer>().material = totemMatGlow;
            if (triggered) return;

            float timeChange = movementScript.ex.ExhibitionMode ? 15 : -15;
            movementScript.timer += timeChange;

            movementScript.respawnPoint = respawnPoint;
            respawnPlayer.voicePlayer = voiceSource;
            totem.Play();
            totem.gameObject.GetComponent<AudioSource>().Play();
            particleRock.Play();
            boulderScript.boulderRespawnTime = boulderTime;
            triggered = true;

        }
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
        Vector3 idolPos = new Vector3(other.transform.position.x, totemPole.transform.position.y, other.transform.position.z);
        Quaternion lookRot = Quaternion.LookRotation(idolPos - totemPole.transform.position, Vector3.up);
        lookRot.x = 0;
        lookRot.z = 0;
        totemPole.transform.rotation = Quaternion.Slerp(totemPole.transform.rotation, lookRot, Time.deltaTime);
        //totemPole.transform.LookAt(new Vector3(other.transform.position.x, totemPole.transform.position.y, other.transform.position.z));
    }

}
