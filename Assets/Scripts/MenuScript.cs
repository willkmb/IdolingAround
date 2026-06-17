using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject trans;
    [SerializeField] GameObject screen;
    [SerializeField] GameObject text;
    [SerializeField] GameObject aboutScreen;
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject tint;
    [SerializeField] AudioSource click;
    [SerializeField] GameObject arrow;
    private bool pressedPlay = false;
    public void playButton()
    {
        if (pressedPlay) return;
        pressedPlay = true;
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
        SceneManager.LoadScene("BlockoutTestV2");
    }

    public void quitButton()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        clicked.GetComponent<AudioSource>().Play();
        Application.Quit();
    }

    public void about()
    {
        aboutScreen.GetComponent<Animation>()["AboutAnimIn"].speed = 1;
        aboutScreen.GetComponent<Animation>()["AboutAnimIn"].time = 0f;
        aboutScreen.GetComponent<Animation>().Play();
        menuScreen.GetComponent<Animation>()["MenuSlideOutAnim"].speed = 1;
        menuScreen.GetComponent<Animation>()["MenuSlideOutAnim"].time = 0f;
        menuScreen.GetComponent<Animation>().Play();
        menuScreen.GetComponent<MenuMove>().enabled = false;
        click.Play();
        tint.GetComponent<Animation>()["TintMenuAnim"].speed = 1;
        tint.GetComponent<Animation>()["TintMenuAnim"].time = 0f;
        tint.GetComponent<Animation>().Play();
        trans.GetComponent<AudioSource>().Play();
    }

    public void aboutBack()
    {
        arrow.GetComponent<Animation>().Play("NextArrowClick");
        click.Play();

        var about = aboutScreen.GetComponent<Animation>();
        about["AboutAnimIn"].speed = -1;
        about["AboutAnimIn"].time = about["AboutAnimIn"].length;
        about.Play("AboutAnimIn");

        var menu = menuScreen.GetComponent<Animation>();
        menu["MenuSlideOutAnim"].speed = -1;
        menu["MenuSlideOutAnim"].time = menu["MenuSlideOutAnim"].length;
        menu.Play("MenuSlideOutAnim");

        trans.GetComponent<AudioSource>().Play();
        var tintAnim = tint.GetComponent<Animation>();
        tintAnim["TintMenuAnim"].speed = -1;
        tintAnim["TintMenuAnim"].time = tintAnim["TintMenuAnim"].length;
        tintAnim.Play("TintMenuAnim");
    }

    private void Update()
    {
        if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
