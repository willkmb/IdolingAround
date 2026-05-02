using System.Collections;
using UnityEngine;

public class ObjectDecay : MonoBehaviour
{
    [HideInInspector] public float timeUntilDecay;
    [HideInInspector] public float mult;
    Animation anim;

    private void OnEnable()
    {
        anim = GetComponent<Animation>();
    }

    public void StartObjDecay()
    {
        Invoke("DecayObj", timeUntilDecay * mult);
    }
    void DecayObj()
    {
        anim.Play();
        Invoke("DisableObj", anim.clip.length);
    }
    void DisableObj()
    {
        gameObject.SetActive(false);
    }

}
