using UnityEngine;

public class BreakVase : MonoBehaviour
{
    [SerializeField] GameObject BrokenVase;
    [SerializeField] float speedToBreak;
    AudioSource sound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(this.GetComponent<Rigidbody>().linearVelocity.magnitude);
        if (this.GetComponent<Rigidbody>().linearVelocity.magnitude >= speedToBreak/2 || 
            (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude >= speedToBreak * 4))
        {
            sound.Play();
            Instantiate(BrokenVase, this.transform.position, this.transform.rotation);
            this.gameObject.SetActive(false);
        }
    }
}
