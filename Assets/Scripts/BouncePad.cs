using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float JumpMult;
    [SerializeField] MovementScript movementScript;
    [SerializeField] Animation anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindFirstObjectByType<MovementScript>().GetComponent<MovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<MovementScript>() != null)
        {
            anim.Play();
            BouncePlayer();
        }

        else
        {
            if (col.gameObject.GetComponent<Rigidbody>() != null)
            {
                anim.Play();
                col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpMult, ForceMode.Impulse);
            }
        }
    }
    void BouncePlayer()
    {
        movementScript.jump(JumpMult);
    }

}
