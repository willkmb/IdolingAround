using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostPlay : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    private List<Vector3> rotations = new List<Vector3>();
    private List<float> timestamp = new List<float>();
    private void Start()
    {
        ghostRecorder.load();
        positions.AddRange(ghostRecorder.positions);
        rotations.AddRange(ghostRecorder.rotations);
        timestamp.AddRange(ghostRecorder.timestamp);
        if (positions.Count > 1 && rotations.Count > 1) StartCoroutine(playback());
        else this.gameObject.SetActive(false);
    }

    IEnumerator playback()
    {
        for (int i = 0; i < positions.Count - 1; i++)
        {
            float interpolateTime = timestamp[i+1] - timestamp[i];
            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime / interpolateTime;
                if (i + 1 >= positions.Count)
                {
                    this.gameObject.SetActive(false);
                    yield break;
                }
                transform.position = Vector3.Lerp(positions[i], positions[i+1], time);
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(rotations[i]), Quaternion.Euler(rotations[i + 1]), time);
                yield return null;
            }
        }
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            positions.Clear();
            rotations.Clear();
            timestamp.Clear();
            PlayerPrefs.DeleteKey("ghostPos");
            PlayerPrefs.DeleteKey("ghostRot");
            PlayerPrefs.DeleteKey("ghostTime");
            PlayerPrefs.Save();
            this.gameObject.SetActive(false);
        }
    }
}
