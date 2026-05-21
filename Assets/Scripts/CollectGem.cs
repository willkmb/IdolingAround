using UnityEngine;

public class CollectGem : MonoBehaviour
{
    GameObject player;
    GemCounter gemCounter;
    AudioSource sfx;
    bool isCollected;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gemCounter = player.GetComponent<GemCounter>();
        sfx = GetComponent<AudioSource>();
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
}
