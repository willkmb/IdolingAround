using Cinemachine.Utility;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    [SerializeField] float speed = 5f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float snapDist = 0.01f;
    [SerializeField] AnimationCurve accCurve;

    Vector3 initialVel;
    Vector3 targetPos;

    float totalDist;
    float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = pointA.transform.position;
        targetPos = pointB.transform.position;
        totalDist = Vector3.Distance(pointA.transform.position, pointB.transform.position);

        StartCoroutine(MovePlatform());
    }

    // Update is called once per frame
    void Update()
    {
    }


    IEnumerator MovePlatform()
    {
        while (true)
        {
            while ((targetPos - transform.position).sqrMagnitude > snapDist)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

                yield return null;
            }
            targetPos = targetPos == pointA.transform.position ? pointB.transform.position : pointA.transform.position;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
