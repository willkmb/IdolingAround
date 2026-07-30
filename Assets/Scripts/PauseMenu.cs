using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] MovementScript move;
    [SerializeField] CamPedestal pause;
    private bool paused = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                move.enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                StartCoroutine(fadeSFX());
                pause.callPauseScreen();
            }
            else
            {
                pause.callPauseScreen();
            }

            paused = !paused;
        }
    }

    IEnumerator fadeSFX()
    {
        while (move.rolling.volume > 0)
        {
            move.rolling.volume = Mathf.MoveTowards(move.rolling.volume, 0, 0.5f * Time.deltaTime);
            yield return null;
        }
    }
}
