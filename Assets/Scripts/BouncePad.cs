using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float JumpMult;
    [SerializeField] MovementScript movementScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Bounce();
    }
    void Bounce()
    {
        movementScript.jump(JumpMult);
    }
}
