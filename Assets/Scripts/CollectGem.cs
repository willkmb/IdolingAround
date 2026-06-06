using UnityEngine;

public class CollectGem : MonoBehaviour
{
    [SerializeField] GameObject[] gems;
    GameObject player;
    GemCounter gemCounter;
    AudioSource sfx;
    bool isCollected;
    private void Start()
    {
        gems[Random.Range(0, gems.Length - 1)].SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        gemCounter = player.GetComponent<GemCounter>();
        sfx = GetComponent<AudioSource>();
        Invoke("TriggerOn", 0.5f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isCollected)
        {
            gemCounter.UpdateGemCounter();
            sfx.Play();
            this.gameObject.SetActive(false);
            isCollected = true;
        }

    }

    void TriggerOn()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
