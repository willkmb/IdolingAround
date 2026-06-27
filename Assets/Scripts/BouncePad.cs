using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float JumpMult;
    [SerializeField] float ObjBounceMult;
    MovementScript movementScript;
    [SerializeField] Animation anim;
    Collider objCol;
    AudioSource padSFX;

    private bool hasBounced;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
        padSFX = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (hasBounced) return;
        if (col.gameObject.GetComponent<MovementScript>() != null)
        {
            BouncePlayer();
            anim.Play();
            hasBounced = true;
        }

        else if (col.gameObject.GetComponent<Rigidbody>() != null)
        {
            objCol = col;
            BounceObject();
            anim.Play();
        }
    }

    private void OnTriggerExit(Collider other) { hasBounced = false; }

    void BouncePlayer()
    {
        Rigidbody rb = movementScript.gameObject.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpMult, rb.linearVelocity.z);
        //float pitch = Random.Range(0.80f, 1f);
        //padSFX.pitch = pitch;
        padSFX.Play();
        //movementScript.sourceJump.pitch = pitch;
        //movementScript.sourceJump.Play();
    }

    void BounceObject()
    {
        padSFX.Play();
        Rigidbody rb = objCol.gameObject.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, ObjBounceMult, rb.linearVelocity.z);
    }
}
