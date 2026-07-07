using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    [SerializeField] float sinkSpeed;
    [SerializeField] float sinkAmount;

    bool isSinking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSinking && transform.localPosition.y > sinkAmount)
        {
            transform.Translate(Vector3.down * Time.deltaTime * sinkSpeed);
        }
        else if (!isSinking && transform.localPosition.y <= 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * sinkSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isSinking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isSinking = false;
        }
    }
}
