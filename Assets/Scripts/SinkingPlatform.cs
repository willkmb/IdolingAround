using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    [SerializeField] float sinkSpeed;
    [SerializeField] float sinkAmount;

    bool isSinking;


    Vector3 currentPos;
    Vector3 lastPos;
    Vector3 posDifference;
    MovementScript movementScript;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = FindFirstObjectByType<MovementScript>();
        rb = movementScript.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //currentPos = transform.position;
        if (isSinking && transform.localPosition.y > sinkAmount)
        {
            transform.Translate(Vector3.down * Time.deltaTime * sinkSpeed);
        }
        else if (!isSinking && transform.localPosition.y <= 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * sinkSpeed * 2);
        }
        //posDifference = currentPos - lastPos;
        //lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isSinking = true;
            Invoke("CanNotSquash", 0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isSinking = false;
            movementScript.canSquashAndStretch = true;
        }
    }

    void CanNotSquash()
    {
        movementScript.canSquashAndStretch = false;
    }
}
