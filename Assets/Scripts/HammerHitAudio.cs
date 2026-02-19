using UnityEngine;

public class HammerHitAudio : MonoBehaviour
{
    AudioSource sound;
    [SerializeField] Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(rb.linearVelocity.magnitude);
        sound.volume = 0.5f * rb.linearVelocity.magnitude/8;
        sound.pitch = Random.Range(0.9f, 1.1f);
        sound.Play();
    }
}
