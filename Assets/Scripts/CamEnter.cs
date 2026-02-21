using System.Collections;
using UnityEngine;

public class CamEnter : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject thisCam;
    [SerializeField] Animation anim;
    [SerializeField] Animation trans;
    [SerializeField] GameObject text;
    BoxCollider col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider>();
        Cursor.visible = false;
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

    IEnumerator CamSwitch()
    {
        //Debug.Log("idol black screen");
        trans.Play();
        trans.gameObject.GetComponent<AudioSource>().Play();
        //Time.timeScale = 0;
        yield return new WaitForSeconds(0.5f);
        RenderSettings.fogDensity = 0.015f;
        //mainCam.SetActive(false);
        thisCam.SetActive(true);
        anim.Play();
        text.SetActive(false);
        yield return new WaitForSeconds(2f);
        trans.Play();
        yield return new WaitForSeconds(.5f);
        //Debug.Log("idol black screen2");
        col.enabled = false;
        RenderSettings.fogDensity = 0.05f;
        //mainCam.SetActive(true);
        thisCam.SetActive(false);
        //Time.timeScale = 1;
    }
}
