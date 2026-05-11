using System.Linq;
using UnityEngine;

public class CrushingWalls : MonoBehaviour
{
    MovementScript playerMovement;
    [SerializeField] Animation deathAnim;
    public Animation[] anims;
    public TriggerScript[] triggers;
    bool hasAnimStarted;
    bool idolCrushed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = FindFirstObjectByType<MovementScript>();
        anims = GetComponentsInChildren<Animation>();
        triggers = GetComponentsInChildren<TriggerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCrushed();
    }


    void CheckCrushed()
    {
        if (hasAnimStarted)
        {
            if (!idolCrushed)
            {
                if (triggers.All(triggers => triggers.inTrigger == true))
                {
                    foreach (var anim in anims)
                    {
                        //anim.Stop();
                    }
                    GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = true;
                    playerMovement.gameObject.transform.position = playerMovement.respawnPoint.transform.position;
                    GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = false;
                    //deathAnim.Play();
                    Debug.Log("Idol is crushed");
                    idolCrushed = true;
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(!hasAnimStarted)
            {
                foreach(var anim in anims)
                {
                    anim.Play();
                }
                hasAnimStarted = true;
            }
        }
    }
}
