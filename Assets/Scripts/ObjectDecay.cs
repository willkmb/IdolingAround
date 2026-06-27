using System.Collections;
using UnityEngine;

public class ObjectDecay : MonoBehaviour
{
    public float timeUntilDecay;
    public float mult;
    Animation anim;

    private void OnEnable()
    {
        anim = GetComponent<Animation>();
        Invoke("StartObjDecay", 0.1f);
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
