using System.Collections;
using UnityEngine;

public class VoiceLinesBoulderStartRoll : MonoBehaviour
{
    [SerializeField] Animation boulderRoll;
    [SerializeField] AudioClip[] audioClips;
    AudioSource voiceSource;
    private void OnEnable()
    {
        voiceSource = GetComponent<AudioSource>();
        StartCoroutine(Cutscene());
    }

    public IEnumerator Cutscene()
    {
        voiceSource.clip = audioClips[0];
        voiceSource.Play();
        yield return new WaitForSeconds[(int)audioClips[0].length];
        voiceSource.clip = audioClips[1];
        voiceSource.Play();
        yield return new WaitForSeconds[(int)audioClips[1].length];
        voiceSource.clip = audioClips[2];
        voiceSource.Play();
        yield return new WaitForSeconds[(int)audioClips[2].length];
        voiceSource.clip = audioClips[3];
        voiceSource.Play();
        yield return new WaitForSeconds[(int)audioClips[3].length];
        voiceSource.clip = audioClips[4];
        voiceSource.Play();
        yield return new WaitForSeconds[(int)audioClips[4].length];
        voiceSource.clip = audioClips[5];
        voiceSource.Play();
        yield return new WaitForSeconds[(int)audioClips[5].length];
        boulderRoll.Play();
        yield return null;
    }
}
