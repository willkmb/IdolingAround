using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PotSmash : MonoBehaviour
{
    public ObjectDecay[] childObjDecay;
    AudioSource sound;
    public bool decaysOverTime;
    public float timeUntilDecay;
    public Vector3 potVelocity;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;
    [SerializeField] float minDecayMult;
    [SerializeField] float maxDecayMult;


    private void OnEnable()
    {
        sound = GetComponent<AudioSource>();
        childObjDecay = GetComponentsInChildren<ObjectDecay>();
        sound.pitch = Random.Range(minPitch, maxPitch);
        sound.Play();
        StartCoroutine(ObjStartDecay());
    }

    IEnumerator ObjStartDecay()
    {
        if (decaysOverTime)
        {
            /*
            int decayInt = Random.Range(0, 2);
            if (decayInt == 0)
            {
                decaysOverTime = true;
            }
            else
            {
                decaysOverTime = false;
            }
            */

            if (decaysOverTime)
            {

                foreach (ObjectDecay decay in childObjDecay)
                {
                    Debug.Log("setting vase shard");
                    Random.InitState((int)DateTime.Now.Ticks);
                    decay.GetComponent<Rigidbody>().linearVelocity = potVelocity;
                    decay.timeUntilDecay = timeUntilDecay;
                    float mult = Random.Range(minDecayMult, maxDecayMult);
                    decay.mult = mult;
                }
            }
            yield return null;
        }



    }
}
