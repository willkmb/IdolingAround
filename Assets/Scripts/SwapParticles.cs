using UnityEngine;

public class SwapParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem[] leaves;
    [SerializeField] ParticleSystem embers;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6)
        {
            foreach (ParticleSystem p in leaves)
            {
                p.Stop();
            }
            embers.Play();
        }
    }

    private void Start()
    {
        if (MenuScript.corridorStart)
        {
            foreach (ParticleSystem p in leaves)
            {
                p.Stop();
            }
            embers.Play();
        }
    }
}
