using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnTrigger : MonoBehaviour
{
    GameObject Player;
    MovementScript movementScript;
    RespawnPlayer respawnPlayer;
    [SerializeField] AudioSource DeathSound;
    [SerializeField] GameObject thisCutscene;
    [SerializeField] float cutsceneLength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        movementScript = Player.GetComponent<MovementScript>();
        respawnPlayer = Player.GetComponent<RespawnPlayer>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (movementScript.isSpawning == false)
        {
            if (col.gameObject.layer == 6)
            {
                DeathSound.Play();
                if (thisCutscene != null)
                {
                    respawnPlayer.cutsceneLength = cutsceneLength;
                    respawnPlayer.cutscene = thisCutscene;
                }
                respawnPlayer.StartSpawn();
            }
        }
    }



}
