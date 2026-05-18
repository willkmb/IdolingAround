using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimationScript : MonoBehaviour
{
    public Image[] frames;
    public float frameDelay = 0.1f;

    private int curFrame = 0;

    void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        while (true)
        {
            for (int i = 0; i < frames.Length; i++) frames[i].gameObject.SetActive(false);
            frames[curFrame].gameObject.SetActive(true);
            yield return new WaitForSeconds(frameDelay);
            curFrame++;
            if (curFrame >= frames.Length) curFrame = 0;
        }
    }
}
