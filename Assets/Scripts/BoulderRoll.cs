using UnityEditor.Splines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BoulderRoll : MonoBehaviour
{
    [HideInInspector] public Transform boulderRespawn;
    [HideInInspector] public float boulderRespawnTime;
    SplineAnimate splineAnim;

    Rigidbody rb;

    float targetDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = GetComponent<SplineAnimate>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //RollBoulder();
    }

    void RollBoulder()
    {
        

        /*
        targetDistance = Vector3.Distance(navAgent.transform.position, target.position);
        if (targetDistance < killDistance)
        {
            navAgent.isStopped = true;
            //anim.speed = 0;
            navAgent.transform.position = boulderRespawn.position;
        }

        else
        {
            navAgent.isStopped = false;
            navAgent.destination = target.position;
            transform.position = new Vector3(navAgent.transform.position.x, transform.position.y, navAgent.transform.position.z);
            transform.rotation = navAgent.transform.rotation;
            anim.speed = 1;
        }
        */


    }

    public void RespawnBoulder()
    {
        splineAnim.Pause();
        splineAnim.ElapsedTime = boulderRespawnTime;
        splineAnim.Play();
    }
}
