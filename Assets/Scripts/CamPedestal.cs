using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    BoxCollider col;
    private bool gamefin = false;
    void Start() { col = GetComponent<BoxCollider>(); }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gamefin) restart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            StartCoroutine(CamSwitch());
        }

    }

    IEnumerator CamSwitch()
    {
        Debug.Log("idol black screen");
        
        mainCam.SetActive(false);
        thisCam.SetActive(true);
        idol.GetComponent<MovementScript>().CheckScore();
        idol.GetComponent<MovementScript>().enabled = false;
        idol.GetComponent<Rigidbody>().isKinematic = true;
        thisCam.GetComponent<Animation>().Play();
        idol.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(3f);
        
        trans.GetComponent<Animation>().Play("TransIn");
        yield return new WaitForSeconds(0.9f);
        screenTint.Play();
        yield return new WaitForSeconds(0.3f);
        screenTint.gameObject.SetActive(false);
        trans.GetComponent<Animation>().Play("Transout");
        endCam.SetActive(true);
        thisCam.SetActive(false);
        Destroy(oldUIAnim);
        foreach (var ui in oldUI) Destroy(ui);
        newUI.SetActive(true);
    }

    void restart()
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
    }
}
