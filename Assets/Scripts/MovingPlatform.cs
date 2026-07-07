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
    Vector3 midPos;
    Rigidbody rb;
    float totalDist;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = pointA.transform.position;
        targetPos = pointB.transform.position;
        var midPosX = (pointA.transform.position.x + pointB.transform.position.x) / 2;
        var midPosY = (pointA.transform.position.y + pointB.transform.position.y) / 2;
        var midPosZ = (pointA.transform.position.z + pointB.transform.position.z) / 2;
        midPos = new Vector3(midPosX, midPosY, midPosZ);
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
            float dist = Vector3.Distance(transform.position, midPos);
            float distPerc = Mathf.Clamp(1 - (dist / (totalDist / 2)), 0.5f, 1f);
            Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime * distPerc);
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
}
