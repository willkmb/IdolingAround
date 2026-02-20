using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject trans;
    [SerializeField] GameObject screen;
    [SerializeField] GameObject text;
    public void playButton()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        clicked.GetComponent<Animation>().Play();
        clicked.GetComponent<AudioSource>().Play();
        trans.GetComponent<Animation>().Play();
        trans.GetComponent<AudioSource>().Play();
        Invoke("load", trans.GetComponent<Animation>().clip.length);
    }

    void load()
    {
        screen.GetComponent<Animation>().Play();
        Invoke("load2", 0.35f);
    }

    void load2()
    {
        text.GetComponent<Animation>().Play();
        screen.GetComponent<AudioSource>().Play();
        Invoke("load3", screen.GetComponent<AudioSource>().clip.length + 1f);
    }

    void load3()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void quitButton()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        clicked.GetComponent<AudioSource>().Play();
        Application.Quit();
    }
}
