using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PresenceUIFirebase : MonoBehaviour
{
    public InputField nameInput;
    public Toggle workingToggle;
    string dbURL = "https://idolingaround-userstatus-default-rtdb.europe-west1.firebasedatabase.app";

    void Start()
    {
        StartCoroutine(SendLoop());
    }

    IEnumerator SendLoop()
    {
        while (true)
        {
            if (!string.IsNullOrEmpty(nameInput.text))
            {
                string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

                string json =
                    "{"
                    + "\"scene\":\"" + scene + "\","
                    + "\"active\":" + workingToggle.isOn.ToString().ToLower()
                    + "}";

                string url = dbURL + "presence/" + nameInput.text + ".json";

                UnityWebRequest req = new UnityWebRequest(url, "PUT");
                byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

                req.uploadHandler = new UploadHandlerRaw(body);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");

                yield return req.SendWebRequest();
            }

            yield return new WaitForSeconds(2);
        }
    }
}