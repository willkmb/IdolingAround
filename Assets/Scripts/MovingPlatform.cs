using Cinemachine.Utility;
using System.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    [SerializeField] float speed = 5f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float snapDist = 0.01f;

    Vector3 targetPos;
    Rigidbody rb;
    float totalDist;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = pointA.transform.position;
        targetPos = pointB.transform.position;
        totalDist = Vector3.Distance(pointA.transform.position, pointB.transform.position);
        rb = GetComponent<Rigidbody>();

        //StartCoroutine(MovePlatform());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        
        if ((targetPos - transform.position).sqrMagnitude > snapDist)
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else
        {
            if (timer < waitTime)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                targetPos = targetPos == pointA.transform.position ? pointB.transform.position : pointA.transform.position;
                timer = 0f;
            }
        }
        
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            while ((targetPos - transform.position).sqrMagnitude > snapDist)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                rb.MovePosition(newPos);
                yield return null;
            }
            targetPos = targetPos == pointA.transform.position ? pointB.transform.position : pointA.transform.position;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
