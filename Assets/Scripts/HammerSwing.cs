using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class HammerSwing : MonoBehaviour
{
    [SerializeField] float waitForTime;
    Animation anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animation>();
        Invoke("StartAnim", waitForTime);
    }

    void StartAnim()
    {
        anim.Play();
    }
}
