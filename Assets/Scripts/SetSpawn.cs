using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    private MovementScript movementScript;
    RespawnPlayer respawnPlayer;
    private AudioSource sound;

    [Header("Totem")]
    [SerializeField] private ParticleSystem particleRock;
    [SerializeField] private Transform respawnPoint;
    public Animation totem;
    [SerializeField] GameObject totemPole;
    [SerializeField] Material totemMat;
    [SerializeField] Material totemMatGlow;

    private bool triggered = false;

    [Header("Audio")]
    [SerializeField] AudioSource voiceSource;
    void Start()
    {
        movementScript = FindFirstObjectByType<MovementScript>();
        respawnPlayer = FindFirstObjectByType<RespawnPlayer>();
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        totemPole.GetComponentInChildren<MeshRenderer>().material = totemMatGlow;

        if (other.gameObject.layer != 6) return;
        if (triggered) return;

        /*
        float timeChange = movementScript.ex.ExhibitionMode ? 15 : -15;
        movementScript.timer += timeChange;
        */

        movementScript.respawnPoint = respawnPoint;
        respawnPlayer.voicePlayer = voiceSource;
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
        Quaternion lookRot = Quaternion.LookRotation(other.transform.position - totemPole.transform.position, Vector3.up);
        lookRot.x = 0;
        lookRot.z = 0;
        totemPole.transform.rotation = Quaternion.Slerp(totemPole.transform.rotation, lookRot, Time.deltaTime);
        //totemPole.transform.LookAt(new Vector3(other.transform.position.x, totemPole.transform.position.y, other.transform.position.z));
    }
}