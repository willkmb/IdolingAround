using UnityEngine;

public class BreakVase : MonoBehaviour
{
    [SerializeField] GameObject BrokenVase;
    [SerializeField] float speedToBreak;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(this.GetComponent<Rigidbody>().linearVelocity.magnitude);
        if (this.GetComponent<Rigidbody>().linearVelocity.magnitude >= speedToBreak/2 || 
            (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude >= speedToBreak * 4))
        {
            Instantiate(BrokenVase, this.transform.position, this.transform.rotation);
            this.gameObject.SetActive(false);
        }
    }
}
