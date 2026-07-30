using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] MovementScript move;
    [SerializeField] CamPedestal pause;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider volSliderSFX;
    [SerializeField] Slider volSliderMusic;
    [SerializeField] Toggle toggle;
    private bool paused = false;

    void Start()
    {
        volSliderMusic.value = 1f;
        volSliderSFX.value = 1f;
        SetMusicVolume(1f);
        SetSFXVolume(1f);
        volSliderMusic.onValueChanged.AddListener(SetMusicVolume);
        volSliderSFX.onValueChanged.AddListener(SetSFXVolume);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                move.enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                StartCoroutine(fadeSFX());
                pause.callPauseScreen();
                GhostPauser.PauseGhosts();
            }
            else
            {
                pause.callPauseScreenOff();
            }

            paused = !paused;
        }

        if (toggle.isOn) move.inverted = true;
        else { move.inverted = false; }
    }

    IEnumerator fadeSFX()
    {
        while (move.rolling.volume > 0)
        {
            move.rolling.volume = Mathf.MoveTowards(move.rolling.volume, 0, 0.5f * Time.deltaTime);
            yield return null;
        }
    }

    public void SetMusicVolume(float value) { mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20); }

    public void SetSFXVolume(float value) { mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20); }

}
