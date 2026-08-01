using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamPedestal : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject thisCam;
    [SerializeField] GameObject endCam;
    [SerializeField] GameObject idol;
    [SerializeField] GameObject trans;
    [SerializeField] Animation screenTint;
    [SerializeField] GameObject[] oldUI;
    [SerializeField] UIAnimationScript oldUIAnim;
    [SerializeField] GameObject newUI;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject nextArrow;
    [SerializeField] GameObject curText;
    [SerializeField] GameObject highscore;
    [SerializeField] GameObject[] gemDeath;
    [SerializeField] GameObject[] gemDeathText;
    [SerializeField] GameObject leaderboard;
    [SerializeField] GameObject[] fadeObjectsLB;
    [SerializeField]GameObject[] fadeTextLB;
    [SerializeField] GameObject[] ghostButtons;
    [SerializeField] GameObject spacing;
    [SerializeField] AudioSource transSound;
    [SerializeField] GameObject restartText;
    [SerializeField] GameObject jumpCharge;

    [SerializeField] MovementScript movementScript;
    [SerializeField] checkProgressScript dist;
    [SerializeField] PauseMenu pause;

    BoxCollider col;
    PauseMenu pauseMenu;

    private bool gamefin = false;
    private bool needToClick = false;
    private bool canRestart = false;
    private bool hasEnded = false;
    void Start() 
    { 
        col = GetComponent<BoxCollider>(); 
        pauseMenu = FindFirstObjectByType<PauseMenu>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gamefin) restart();
        if(canRestart && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(reloadScene());
        }
        if (!needToClick) return;
        if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && !hasEnded)
        {
            movementScript.canRespawn = false;
            StartCoroutine(CamSwitch());
            hasEnded = true;
        }

    }

    public void callEndScreen()
    {
        if (hasEnded) return;
        dist.ended = true;
        movementScript.canRespawn = false;
        StartCoroutine(CamSwitch());
        hasEnded = true;
    }


    IEnumerator CamSwitch()
    {
        Debug.Log("idol black screen");

        pause.canPause = false;
        jumpCharge.SetActive(false);
        mainCam.SetActive(false);
        thisCam.SetActive(true);
        idol.GetComponent<RespawnPlayer>().enabled = false;
        idol.GetComponent<MovementScript>().CheckScore();
        idol.GetComponent<MovementScript>().enabled = false;
        idol.GetComponent<Rigidbody>().isKinematic = true;
        thisCam.GetComponent<Animation>().Play();
        idol.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(3f);
        
        trans.GetComponent<Animation>().Play("TransIn");
        transSound.Play();
        yield return new WaitForSeconds(0.9f);
        screenTint.Play();
        yield return new WaitForSeconds(0.3f);
        screenTint.gameObject.SetActive(false);
        trans.GetComponent<Animation>().Play("Transout");
        transSound.Play();
        endCam.SetActive(true);
        thisCam.SetActive(false);
        Destroy(oldUIAnim);
        foreach (var ui in oldUI) Destroy(ui);
        newUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //nextArrow.GetComponent<Animation>().Blend("NextArrowFadeIn");
        needToClick = true;
        movementScript.enabled = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LB());
    }

    void restart()
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
    }

    public void nextArrowClick()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        nextArrow.GetComponent<Animation>().Play("NextArrowClick");
        nextArrow.GetComponent<AudioSource>().Play();
        StartCoroutine(LB());
        
    }

    IEnumerator LB()
    {
        yield return new WaitForSeconds(0.1f);
        nextArrow.GetComponent<Animation>().Play("NextArrowFadeOut");
        yield return new WaitForSeconds(0.2f);
        curText.GetComponent<Animation>().Play();
        highscore.GetComponent<Animation>().Play();
        nextArrow.GetComponentInParent<Button>().enabled = false;
        foreach(var go in gemDeath) go.GetComponent<Animation>().Play();
        foreach(var tmp in gemDeathText) tmp.GetComponent<Animation>().Play();
        leaderboard.GetComponent<Animation>().Play();
        spacing.GetComponent<Animation>().Play();
        foreach (var fo in fadeObjectsLB) fo.GetComponent<Animation>().Play();
        foreach (var ft in fadeTextLB) ft.GetComponent<Animation>().Play();
        foreach (var button in ghostButtons) button.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        restartText.GetComponent<Animation>().Play();
        canRestart = true;
    }
    IEnumerator reloadScene()
    {
        trans.GetComponent<Animation>().Play("TransIn");
        transSound.Play();
        yield return new WaitForSeconds(0.9f);
        screenTint.Play();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void callEndScreenEx()
    {
        if (hasEnded) return;
        dist.ended = true;
        movementScript.canRespawn = false;
        movementScript.enabled = false;
        StartCoroutine(endScreenEx());
        hasEnded = true;
    }

    IEnumerator endScreenEx()
    {
        pause.canPause = false;
        jumpCharge.SetActive(false);
        idol.GetComponent<MovementScript>().CheckScore();
        trans.GetComponent<Animation>().Play("TransIn");
        transSound.Play();
        yield return new WaitForSeconds(0.9f);
        screenTint.Play();
        yield return new WaitForSeconds(0.3f);
        screenTint.gameObject.SetActive(false);
        trans.GetComponent<Animation>().Play("Transout");
        transSound.Play();
        Destroy(oldUIAnim);
        foreach (var ui in oldUI) Destroy(ui);
        newUI.SetActive(true);
        needToClick = true;
        yield return new WaitForSeconds(0.75f);
        if(!MenuScript.corridorStart) StartCoroutine(LB());
        yield return new WaitForSeconds(1.25f);
        restartText.GetComponent<Animation>().Play();
        canRestart = true;
    }

    public void callPauseScreen()
    {
        StartCoroutine(pauseScreen());
    }

    public void callPauseScreenOff()
    {
        StartCoroutine(pauseScreenOff());
    }

    IEnumerator pauseScreen()
    {
        trans.GetComponent<Animation>().Play("TransIn");
        transSound.Play();
        yield return new WaitForSeconds(0.9f);
        screenTint.Play();
        yield return new WaitForSeconds(0.3f);
        screenTint.gameObject.SetActive(false);
        trans.GetComponent<Animation>().Play("Transout");
        transSound.Play();
        oldUIAnim.enabled = false;
        foreach (var ui in oldUI) ui.SetActive(false);
        PauseUI.SetActive(true);
        pauseMenu.paused = true;
        needToClick = true;
    }
    IEnumerator pauseScreenOff()
    {
        trans.GetComponent<Animation>().Play("TransIn");
        transSound.Play();
        yield return new WaitForSeconds(0.9f);
        screenTint.Play();
        yield return new WaitForSeconds(0.3f);
        screenTint.gameObject.SetActive(false);
        trans.GetComponent<Animation>().Play("Transout");
        transSound.Play();
        oldUIAnim.enabled = true;
        foreach (var ui in oldUI) ui.SetActive(true);
        PauseUI.SetActive(false);
        needToClick = false;
        Cursor.visible = false;
        movementScript.enabled = true;
        GhostPauser.ResumeGhosts();
        idol.GetComponent<Rigidbody>().isKinematic = false;
        pauseMenu.paused = false;
    }

}
