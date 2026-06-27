using System.Collections;
using UnityEngine;

public class CollectGem : MonoBehaviour
{
    [SerializeField] GameObject[] gems;
    GameObject player;
    GemCounter gemCounter;
    public AudioSource sfx;
    bool isCollected;
    private void Start()
    {
        gems[Random.Range(0, gems.Length - 1)].SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        gemCounter = player.GetComponent<GemCounter>();
        sfx = GetComponent<AudioSource>();
        StartCoroutine(TriggerOn());
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isCollected)
        {
            sfx.Play();
            gemCounter.UpdateGemCounter();
            this.gameObject.SetActive(false);
            isCollected = true;
        }

    }

    IEnumerator TriggerOn()
    {
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<BoxCollider>().enabled = true;
        yield return null;
    }
}
