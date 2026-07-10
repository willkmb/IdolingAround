using System.Collections;
using System.Linq;
using System.Net;
using UnityEngine;

public class CrushingWalls : MonoBehaviour
{
    RespawnPlayer respawnScript;
    [SerializeField] float speed;

    [Header("Set In Prefab, Do Not Change")]
    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject midPoint1;
    [SerializeField] GameObject midPoint2;
    [SerializeField] TriggerScript[] crushTriggers;
    [SerializeField] TriggerScript throughTrigger;
    bool hasStartedMove;
    bool idolThrough;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnScript = FindFirstObjectByType<RespawnPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!idolThrough)
        {
            CheckCrushed();

            if (throughTrigger.inTrigger)
            {
                idolThrough = true;
            }

        }

        if (hasStartedMove)
        {
            WallsMoveIn();
        }
        else
        {
            WallsMoveOut();
        }

    }


    void CheckCrushed()
    {
        if (hasStartedMove)
        {
            if (crushTriggers.All(triggers => triggers.inTrigger == true))
            {
                respawnScript.StartSpawn();
                hasStartedMove = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(!hasStartedMove)
            {
                hasStartedMove = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hasStartedMove && !idolThrough)
            {
                hasStartedMove = false;
            }
        }
    }
    void WallsMoveIn()
    {
        if (Vector3.Distance(walls[0].transform.localPosition, midPoint1.transform.localPosition) > 0.05f)
        {
            walls[0].transform.localPosition = Vector3.MoveTowards(walls[0].transform.localPosition, midPoint1.transform.localPosition, speed * Time.deltaTime);
        }
        else { walls[0].transform.localPosition = midPoint1.transform.localPosition; }

        if (Vector3.Distance(walls[1].transform.localPosition, midPoint2.transform.localPosition) > 0.05f)
        {
            walls[1].transform.localPosition = Vector3.MoveTowards(walls[1].transform.localPosition, midPoint2.transform.localPosition, speed * Time.deltaTime);
        }
        else { walls[1].transform.localPosition = midPoint2.transform.localPosition; }

    }

    void WallsMoveOut()
    {
        if (Vector3.Distance(walls[0].transform.localPosition, Vector3.zero) > 0.05f)
        {
            walls[0].transform.localPosition = Vector3.MoveTowards(walls[0].transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        }
        else { walls[0].transform.localPosition = Vector3.zero; }

        if (Vector3.Distance(walls[1].transform.localPosition, midPoint2.transform.localPosition) > 0.05f)
        {
            walls[1].transform.localPosition = Vector3.MoveTowards(walls[1].transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        }
        else { walls[1].transform.localPosition = Vector3.zero; }
    }

}
