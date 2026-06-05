using UnityEngine;
using UnityEngine.Splines;

public class StartBoulder : MonoBehaviour
{
    SplineAnimate splineAnim;
    [SerializeField] GameObject Boulder;
    [SerializeField] Animation boulderAnim;
    bool hasStarted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = Boulder.GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasStarted)
        {
            splineAnim.Play();
            boulderAnim.Play();
            hasStarted = true;
        }
    }
}
