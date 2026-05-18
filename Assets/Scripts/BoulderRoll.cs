using UnityEditor.Splines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BoulderRoll : MonoBehaviour
{
    [HideInInspector] public Transform boulderRespawn;
    [HideInInspector] public float boulderRespawnTime;
    SplineAnimate splineAnim;
    Animation rollAnim;

    Rigidbody rb;

    float targetDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = GetComponent<SplineAnimate>();
        rb = GetComponent<Rigidbody>();
        rollAnim = GetComponentInChildren<Animation>();
    }

    private void FixedUpdate()
    {
        if (splineAnim.Duration - splineAnim.ElapsedTime < 0.1f)
        {
            rollAnim.Stop();
        }
    }

    public void RespawnBoulder()
    {
        if (splineAnim.ElapsedTime != splineAnim.Duration)
        {
            splineAnim.Pause();
            splineAnim.ElapsedTime = boulderRespawnTime;
            splineAnim.Play();
        }
    }
}
