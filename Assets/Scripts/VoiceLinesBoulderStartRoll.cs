using System.Collections;
using UnityEngine;

public class VoiceLinesBoulderStartRoll : MonoBehaviour
{
    [SerializeField] Animation boulderRock;
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
        boulderRock.Play();
        yield return null;

        //1.Loop through each AudioClip
        for (int i = 0; i < audioClips.Length; i++)
        {
            //2.Assign current AudioClip to audiosource
            voiceSource.clip = audioClips[i];

            //3.Play Audio
            voiceSource.Play();

            if (audioClips[i] == audioClips[audioClips.Length - 1])
            {
                boulderRock.Stop();
                boulderRoll.Play();
            }
            //4.Wait for it to finish playing
            while (voiceSource.isPlaying)
            {
                yield return null;
            }

            //5. Go back to #2 and play the next audio in the adClips array
        }

        yield return null;
    }
}
