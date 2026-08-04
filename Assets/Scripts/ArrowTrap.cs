using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private bool randomiseTime;
    [SerializeField] GameObject arrowParent;
    [SerializeField] private List<GameObject> arrows;
    bool hasFired = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform[] childTransforms = arrowParent.GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in childTransforms)
        {
            arrows.Add(childTransform.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasFired)
        { 
            StartCoroutine(FireArrows());
            hasFired = true;
        }
    }

    IEnumerator FireArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            if (arrow == arrows[0]) continue;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            arrow.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
            arrow.GetComponent<AudioSource>().Play();
            float time = randomiseTime ? timeBetweenShots * Random.Range(0.35f, 1.125f) : timeBetweenShots;
            yield return new WaitForSeconds(time);

        }
        
        yield return null;
    }
}
