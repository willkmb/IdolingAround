using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject trans;
    public void playButton()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        clicked.GetComponent<Animation>().Play();
        trans.GetComponent<Animation>().Play();
        Invoke("load", trans.GetComponent<Animation>().clip.length);
    }

    void load()
    {
        SceneManager.LoadScene("GameScene");
    }
}
