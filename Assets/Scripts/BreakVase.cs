using UnityEngine;

public class BreakVase : MonoBehaviour
{
    [SerializeField] GameObject BrokenVase;
    [SerializeField] float speedToBreak;
    [SerializeField] bool decaysOverTime;
    [SerializeField] float timeUntilObjDecay;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(this.GetComponent<Rigidbody>().linearVelocity.magnitude);
        if (this.GetComponent<Rigidbody>().linearVelocity.magnitude >= speedToBreak/2 || 
            (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude >= speedToBreak * 4))
        {
            BrokenVase = Instantiate(BrokenVase, this.transform.position, this.transform.rotation);
            BrokenVase.GetComponent<PotSmash>().timeUntilDecay = timeUntilObjDecay; 
            this.gameObject.SetActive(false);
        }
    }
}
