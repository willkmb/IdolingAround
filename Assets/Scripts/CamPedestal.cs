using System.Collections;
using UnityEngine;

public class CamPedestal : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject thisCam;
    [SerializeField] GameObject CSIdol;
    [SerializeField] GameObject GameIdol;
    [SerializeField] Animation anim;
    [SerializeField] Animation anim2;
    [SerializeField] Animation trans;
    [SerializeField] Animation UI;
    [SerializeField] Animation tint;
    [SerializeField] Animation cur;
    [SerializeField] Animation high;
    [SerializeField] GameObject idol;
    BoxCollider col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            StartCoroutine(CamSwitch());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    IEnumerator CamSwitch()
    {

        Debug.Log("idol black screen");
        idol.GetComponent<MovementScript>().CheckScore();
        trans.Play();
        trans.gameObject.GetComponent<AudioSource>().Play();
        //Time.timeScale = 0;
        yield return new WaitForSeconds(0.5f);
        CSIdol.SetActive(true);
        GameIdol.SetActive(false);
        mainCam.SetActive(false);
        thisCam.SetActive(true);
        anim.Play();
        anim2.Play();
        Debug.Log("idol black screen2 - go to leaderboard");
        col.enabled = false;
        yield return new WaitForSeconds(2.25f);
        UI.Play();
        tint.Play();
        tint.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);
        cur.Play();
        yield return new WaitForSeconds(0.35f);
        high.Play();
    }


}
