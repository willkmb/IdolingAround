using UnityEngine;

public class BreakVase : MonoBehaviour
{
    [SerializeField] GameObject BrokenVase;
    [SerializeField] float speedToBreak;
   
    [SerializeField] bool decaysOverTime;
    [SerializeField] float timeUntilObjDecay;

    [SerializeField] GameObject gem;
    [SerializeField] bool spawnGem;

    bool hasSpawned;
    Vector3 velocityUpdate;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasSpawned)
        {
            if (velocityUpdate.magnitude >= speedToBreak * 2 || 
                (collision.gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude >= speedToBreak))
            {
                BrokenVase = Instantiate(BrokenVase, this.transform.position, this.transform.rotation);
                if(spawnGem) { gem = Instantiate(gem, new Vector3(this.transform.position.x, this.transform.position.y + 0.25f, this.transform.position.z), Quaternion.identity); }
                BrokenVase.GetComponent<PotSmash>().decaysOverTime = decaysOverTime;
                BrokenVase.GetComponent<PotSmash>().timeUntilDecay = timeUntilObjDecay;
                BrokenVase.GetComponent<PotSmash>().potVelocity = velocityUpdate;
                hasSpawned = true;
                this.gameObject.SetActive(false);

            }
        }
    }

    private void FixedUpdate()
    {
        velocityUpdate = this.GetComponentInParent<Rigidbody>().linearVelocity;
    }
}
