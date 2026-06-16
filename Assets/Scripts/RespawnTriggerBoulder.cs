using UnityEngine;

public class RespawnTriggerBoulder : MonoBehaviour
{
    GameObject Player;
    MovementScript movementScript;
    public RespawnPlayer respawnPlayer;
    [SerializeField] BoulderRoll boulderScript;

    //[SerializeField] AudioSource deathSound;
    [SerializeField] GameObject thisCutscene;
    [SerializeField] float cutsceneLength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        movementScript = Player.GetComponent<MovementScript>();

        //boulderScript = GetComponentInParent<BoulderRoll>();
        //deathSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (movementScript.isSpawning == false)
        {
            if (col.gameObject.layer == 6)
            {
                movementScript.isSpawning = true;
                //deathSound.Play();
                if (thisCutscene != null)
                {
                    respawnPlayer.cutsceneLength = cutsceneLength;
                    respawnPlayer.cutscene = thisCutscene;
                    respawnPlayer.StartSpawn();
                    Invoke("SpawnBoulder", cutsceneLength);
                }

            }
        }
    }

    void SpawnBoulder()
    {
        boulderScript.RespawnBoulder();
    }


}
