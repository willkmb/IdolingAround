using UnityEngine;

public class BreakVase : MonoBehaviour
{
    [SerializeField] GameObject BrokenVase;
    [SerializeField] float speedToBreak;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.GetComponent<Rigidbody>().angularVelocity.magnitude >= speedToBreak/2 || 
            (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude >= speedToBreak))
        {
            Instantiate(BrokenVase, this.transform.position, this.transform.rotation);
            this.gameObject.SetActive(false);
        }
    }
}
