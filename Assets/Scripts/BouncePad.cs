using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float JumpMult;
    [SerializeField] float ObjBounceMult;
    MovementScript movementScript;
    [SerializeField] Animation anim;
    Collider objCol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<MovementScript>() != null)
        {
            BouncePlayer();
            anim.Play();
        }

        else
        {
            if (col.gameObject.GetComponent<Rigidbody>() != null)
            {
                objCol = col;
                BounceObject();
                anim.Play();
            }
        }
    }


    void BouncePlayer()
    {
        Vector3 vel = movementScript.gameObject.GetComponent<Rigidbody>().linearVelocity;
        movementScript.gameObject.GetComponent<Rigidbody>().linearVelocity = new Vector3(vel.x, 0, vel.z);
        movementScript.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100 * JumpMult, ForceMode.Impulse);
        float pitch = Random.Range(0.80f, 1f);
        movementScript.sourceJump.pitch = pitch;
        movementScript.sourceJump.Play();
    }

    void BounceObject()
    {
        Rigidbody rb = objCol.gameObject.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * ObjBounceMult, ForceMode.Impulse);
    }
}
