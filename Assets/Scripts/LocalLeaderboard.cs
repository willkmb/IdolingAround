using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalLeaderboard : MonoBehaviour
{
    private TMP_InputField nameInput;
    private List<(string name, float value, string ghostId)> entries = new List<(string, float, string)>();
    private int maxEntries = 8;
    private bool hasEntered = false;
    [SerializeField] TextMeshProUGUI[] entryText;
    [SerializeField] Button[] ghostButtons;
    [SerializeField] ExhibitionVerToggle ex;

    void Start()
    {
        nameInput = GetComponentInChildren<TMP_InputField>();
        nameInput.characterLimit = 4;
        loadEntries();
        RefreshList();

        for (int i = 0; i < ghostButtons.Length; i++)
        {
            int index = i;
            ghostButtons[i].onClick.AddListener(() => playGhost(index));
        }
    }

    public void enterName()
    {
        if (hasEntered) return;
        string name = nameInput.text;
        if (string.IsNullOrEmpty(name)) return;

        string ghostId = System.Guid.NewGuid().ToString();
        ghostRecorder.SaveGhost(ghostId);

        float value;
        if (ex.ExhibitionMode)
        {
            value = PlayerPrefs.GetFloat("LastRunDist", 0f);
            entries.Add((name.ToLower(), value, ghostId));
            entries.Sort((a, b) => b.value.CompareTo(a.value));
        }
        else
        {
            value = PlayerPrefs.GetFloat("LastRunTime", 0f);
            entries.Add((name.ToLower(), value, ghostId));
            entries.Sort((a, b) => a.value.CompareTo(b.value));
        }

        if (entries.Count > maxEntries) entries.RemoveRange(maxEntries, entries.Count - maxEntries);
        saveEntries();
        nameInput.text = "";
        hasEntered = true;
        RefreshList();
    }

    public void playGhost(int index)
    {
        if (index >= entries.Count) return;
        ghostRecorder.selectedId = entries[index].ghostId;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void saveEntries()
    {
        if (ex.ExhibitionMode)
        {
            PlayerPrefs.SetInt("DistLBCount", entries.Count);
            for (int i = 0; i < entries.Count; i++)
            {
                PlayerPrefs.SetString($"DistLBName_{i}", entries[i].name);
                PlayerPrefs.SetFloat($"DistLBValue_{i}", entries[i].value);
                PlayerPrefs.SetString($"DistLBGhost_{i}", entries[i].ghostId);
            }
        }
        else
        {
            PlayerPrefs.SetInt("LeaderBoardCount", entries.Count);
            for (int i = 0; i < entries.Count; i++)
            {
                PlayerPrefs.SetString($"LBName_{i}", entries[i].name);
                PlayerPrefs.SetFloat($"LBTime_{i}", entries[i].value);
                PlayerPrefs.SetString($"LBGhost_{i}", entries[i].ghostId);
            }
        }
        PlayerPrefs.Save();
    }

    void loadEntries()
    {
        entries.Clear();
        if (ex.ExhibitionMode)
        {
            int count = PlayerPrefs.GetInt("DistLBCount", 0);
            for (int i = 0; i < count; i++)entries.Add((PlayerPrefs.GetString($"DistLBName_{i}", ""), PlayerPrefs.GetFloat($"DistLBValue_{i}", 0f), PlayerPrefs.GetString($"DistLBGhost_{i}", "")));
        }
        else
        {
            int count = PlayerPrefs.GetInt("LeaderBoardCount", 0);
            for (int i = 0; i < count; i++)entries.Add((PlayerPrefs.GetString($"LBName_{i}", ""), PlayerPrefs.GetFloat($"LBTime_{i}", 0f), PlayerPrefs.GetString($"LBGhost_{i}", ""))); ;
        }
        RefreshList();
    }

    void RefreshList()
    {
        for (int i = 0; i < entryText.Length; i++)
        {
            if (i < entries.Count)
            {
                if (ex.ExhibitionMode) entryText[i].text = $"{i + 1}. {entries[i].name} - {entries[i].value:F1}m";
                else entryText[i].text = $"{i + 1}. {entries[i].name} - {Mathf.FloorToInt(entries[i].value / 60f):00}:{Mathf.FloorToInt(entries[i].value % 60f):00}";
            }
            else entryText[i].text = $"{i + 1}. ----";

            if (i < ghostButtons.Length)
            {
                bool hasGhost = i < entries.Count && !string.IsNullOrEmpty(entries[i].ghostId);
                ghostButtons[i].interactable = hasGhost;

                Image buttonImg = ghostButtons[i].GetComponent<Image>();
                foreach (Image img in ghostButtons[i].GetComponentsInChildren<Image>())
                {
                    Color col = img.color;
                    col.a = hasGhost ? 1f : 0.5f;
                    img.color = col;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteKey("HighScore");
            PlayerPrefs.DeleteKey("HighscoreDistance");
            PlayerPrefs.DeleteKey("LeaderBoardCount");
            PlayerPrefs.DeleteKey("DistLBCount");
            for (int i = 0; i < maxEntries; i++)
            {
                PlayerPrefs.DeleteKey($"LBName_{i}");
                PlayerPrefs.DeleteKey($"LBTime_{i}");
                PlayerPrefs.DeleteKey($"DistLBName_{i}");
                PlayerPrefs.DeleteKey($"DistLBValue_{i}");
            }
            PlayerPrefs.Save();
            entries.Clear();
            RefreshList();
        }
    }
}
