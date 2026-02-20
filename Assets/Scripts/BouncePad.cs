using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float JumpMult;
    [SerializeField] float ObjBounceMult;
    MovementScript movementScript;
    [SerializeField] Animation anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
    }

    private void OnTriggerEnter(Collider col)
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
                col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpMult * ObjBounceMult, ForceMode.Impulse);
                anim.Play();
            }
        }
    }
    void BouncePlayer()
    {
        movementScript.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100 * JumpMult, ForceMode.Impulse);
        float pitch = Random.Range(0.80f, 1f);
        movementScript.sourceJump.pitch = pitch;
        movementScript.sourceJump.Play();

        //movementScript.jump(JumpMult);
    }

}
