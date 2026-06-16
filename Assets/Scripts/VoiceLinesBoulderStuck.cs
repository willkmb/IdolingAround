using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesBoulderStuck : MonoBehaviour
{
    AudioSource voiceSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] int minTime;
    [SerializeField] int maxTime;

    bool isNearby;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Talk());
        isNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isNearby = false;
    }

    IEnumerator Talk()
    {
        while (isNearby)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            Debug.Log("talking");
            voiceSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
            voiceSource.Play();
        }
        yield return null;
    }
}
