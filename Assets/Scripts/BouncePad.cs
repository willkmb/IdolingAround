using UnityEngine;

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
        movementScript.jump(JumpMult);
    }

}
