using System.Collections;
using UnityEngine;

public class CamEnter : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject thisCam;
    [SerializeField] Animation anim;
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

    IEnumerator CamSwitch()
    {
        Debug.Log("idol black screen");
        //Time.timeScale = 0;
        yield return new WaitForSeconds(0.5f);
        mainCam.SetActive(false);
        thisCam.SetActive(true);
        anim.Play();
        yield return new WaitForSeconds(2.5f);
        Debug.Log("idol black screen2");
        col.enabled = false;
        mainCam.SetActive(true);
        thisCam.SetActive(false);
        //Time.timeScale = 1;
    }
}
