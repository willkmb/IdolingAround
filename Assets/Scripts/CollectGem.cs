using System.Collections;
using UnityEngine;

public class CollectGem : MonoBehaviour
{
    [SerializeField] GameObject[] gems;
    [SerializeField] float speed;
    [SerializeField] float step;
    [SerializeField] float floatDistance;
    GameObject thisGem;
    GameObject player;
    GemCounter gemCounter;
    AudioSource sfx;
    bool isCollected;
    private void Start()
    {
        thisGem = gems[Random.Range(0, gems.Length - 1)];
        thisGem.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        gemCounter = player.GetComponent<GemCounter>();
        sfx = GetComponent<AudioSource>();
        StartCoroutine(TriggerOn());
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < floatDistance && Vector3.Distance(transform.position, player.transform.position) > 0.05f && !isCollected)
        {
            float distPercent = Mathf.Clamp(Vector3.Distance(transform.position, player.transform.position) / floatDistance, 0.25f, 1f);
            step = speed * distPercent * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isCollected)
        {
            sfx.Play();
            gemCounter.UpdateGemCounter();
            StartCoroutine(collectRoutine());
            GetComponent<BoxCollider>().enabled = false;
            isCollected = true;
        }

    }

    IEnumerator collectRoutine()
    {
        thisGem.GetComponent<Animation>().Play("GemCollectAnim");
        yield return new WaitForSeconds(thisGem.GetComponent<Animation>()["GemCollectAnim"].length);
        thisGem.SetActive(false);
    }

    IEnumerator TriggerOn()
    {
        yield return new WaitForSeconds(0.75f);
        this.GetComponent<BoxCollider>().enabled = true;
        yield return null;
    }
}
