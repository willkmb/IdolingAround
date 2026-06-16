using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class StartBoulder : MonoBehaviour
{
    SplineAnimate splineAnim;
    [SerializeField] GameObject Boulder;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject cutscene;
    [SerializeField] Animation boulderAnim;
    [SerializeField] Animation transition1;
    [SerializeField] VoiceLinesBoulderStartRoll startRollVoicelines;
    [SerializeField] VoiceLinesBoulderChase chaseVoicelines;
    bool hasStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = Boulder.GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasStarted)
        {
            mainCamera.SetActive(false);
            cutscene.SetActive(true);
            transition1.Play();
            StartCoroutine(startRollVoicelines.Cutscene());
            StartCoroutine(StartBoulderRoll());
            hasStarted = true;
        }
    }


    IEnumerator StartBoulderRoll()
    {

        yield return new WaitForSeconds(35f);
        transition1.Play();
        cutscene.SetActive(false);
        mainCamera.SetActive(true);
        splineAnim.Play();
        boulderAnim.Play();
        chaseVoicelines.isNearby = true;
        StartCoroutine(chaseVoicelines.Talk());
        yield return null;
    }
}
