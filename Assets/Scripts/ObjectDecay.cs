using System.Collections;
using UnityEngine;

public class ObjectDecay : MonoBehaviour
{
    public float timeUntilDecay;
    Animation anim;

    private void OnEnable()
    {
        anim = GetComponent<Animation>();
        timeUntilDecay = timeUntilDecay * Random.Range(0.75f, 1.5f);
        Invoke("StartObjDecay", 0.1f);
    }

    public void StartObjDecay()
    {
        StartCoroutine(DecayObj());
    }

    IEnumerator DecayObj()
    {
        yield return new WaitForSeconds(timeUntilDecay);
        anim.Play();
        Invoke("DisableObj", anim.clip.length);
    }
    void DisableObj()
    {
        gameObject.SetActive(false);
    }

}
