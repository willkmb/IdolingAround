using UnityEngine;

public class SwapParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem leaves;
    [SerializeField] ParticleSystem embers;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6)
        {
            leaves.Stop();
            embers.Play();
        }
    }
}
