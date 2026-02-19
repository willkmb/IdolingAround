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
        StartCoroutine(CamSwitch());

    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    IEnumerator CamSwitch()
    {

        Debug.Log("idol black screen");
        //Time.timeScale = 0;
        yield return new WaitForSeconds(0.5f);
        CSIdol.SetActive(true);
        GameIdol.SetActive(false);
        mainCam.SetActive(false);
        thisCam.SetActive(true);
        anim.Play();
        anim2.Play();
        yield return new WaitForSeconds(3.5f);
        Debug.Log("idol black screen2 - go to leaderboard");
        col.enabled = false;


        //Time.timeScale = 1;
    }


}
