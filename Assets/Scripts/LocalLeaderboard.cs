using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalLeaderboard : MonoBehaviour
{
    [Header("Menu Preview")]
    [SerializeField] bool allowEntry = true;
    [SerializeField] string gameSceneName = "";

    private TMP_InputField nameInput;
    private List<(string name, float value, string ghostId, int gems, int deaths)> entries = new List<(string, float, string, int, int)>();
    private int maxEntries = 8;
    private bool hasEntered = false;
    [SerializeField] TextMeshProUGUI[] entryText;
    [SerializeField] TextMeshProUGUI[] gemText;
    [SerializeField] TextMeshProUGUI[] deathText;
    [SerializeField] Button[] ghostButtons;
    [SerializeField] ExhibitionVerToggle ex;
    [SerializeField] GameObject player;
    private bool IsExhibition => allowEntry ? ex.ExhibitionMode : true;

    void Start()
    {
        if (allowEntry)
        {
            nameInput = GetComponentInChildren<TMP_InputField>();
            nameInput.characterLimit = 4;
        }
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
        if (!allowEntry || hasEntered) return;
        string name = nameInput.text;
        if (string.IsNullOrEmpty(name)) return;

        string ghostId = System.Guid.NewGuid().ToString();
        ghostRecorder.SaveGhost(ghostId);

        int gems = player.GetComponent<GemCounter>().gems;
        int deaths = player.GetComponent<DeathCounter>().deaths;

        float value;
        if (IsExhibition)
        {
            value = PlayerPrefs.GetFloat("LastRunDist", 0f);
            entries.Add((name.ToLower(), value, ghostId, gems, deaths));
            entries.Sort((a, b) => b.value.CompareTo(a.value));
        }
        else
        {
            value = PlayerPrefs.GetFloat("LastRunTime", 0f);
            entries.Add((name.ToLower(), value, ghostId, gems, deaths));
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
        if (allowEntry) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else SceneManager.LoadScene(gameSceneName);
    }

    void saveEntries()
    {
        if (IsExhibition)
        {
            PlayerPrefs.SetInt("DistLBCount", entries.Count);
            for (int i = 0; i < entries.Count; i++)
            {
                PlayerPrefs.SetString($"DistLBName_{i}", entries[i].name);
                PlayerPrefs.SetFloat($"DistLBValue_{i}", entries[i].value);
                PlayerPrefs.SetString($"DistLBGhost_{i}", entries[i].ghostId);
                PlayerPrefs.SetInt($"DistLBGems_{i}", entries[i].gems);
                PlayerPrefs.SetInt($"DistLBDeaths_{i}", entries[i].deaths);
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
                PlayerPrefs.SetInt($"LBGems_{i}", entries[i].gems);
                PlayerPrefs.SetInt($"LBDeaths_{i}", entries[i].deaths);
            }
        }
        PlayerPrefs.Save();
    }

    void loadEntries()
    {
        entries.Clear();
        if (IsExhibition)
        {
            int count = PlayerPrefs.GetInt("DistLBCount", 0);
            for (int i = 0; i < count; i++)entries.Add((PlayerPrefs.GetString($"DistLBName_{i}", ""), PlayerPrefs.GetFloat($"DistLBValue_{i}", 0f), PlayerPrefs.GetString($"DistLBGhost_{i}", ""),PlayerPrefs.GetInt($"DistLBGems_{i}", 0), PlayerPrefs.GetInt($"DistLBDeaths_{i}", 0)));
        }
        else
        {
            int count = PlayerPrefs.GetInt("LeaderBoardCount", 0);
            for (int i = 0; i < count; i++)entries.Add((PlayerPrefs.GetString($"LBName_{i}", ""), PlayerPrefs.GetFloat($"LBTime_{i}", 0f), PlayerPrefs.GetString($"LBGhost_{i}", ""), PlayerPrefs.GetInt($"LBGems_{i}", 0),PlayerPrefs.GetInt($"LBDeaths_{i}", 0))); ;
        }
        RefreshList();
    }

    void RefreshList()
    {
        for (int i = 0; i < entryText.Length; i++)
        {
            if (i < entries.Count)
            {
                if (IsExhibition) entryText[i].text = $"{i + 1}. {entries[i].name} - {entries[i].value:F1}m";
                else entryText[i].text = $"{i + 1}. {entries[i].name} - {Mathf.FloorToInt(entries[i].value / 60f):00}:{Mathf.FloorToInt(entries[i].value % 60f):00}";
            }
            else entryText[i].text = $"{i + 1}. ----";

            if(i < gemText.Length)
            {
                gemText[i].text = i < entries.Count ? entries[i].gems.ToString() : "-";
                Color gemCol = gemText[i].color;
                gemCol.a = i < entries.Count ? 1f : 0.5f;
                gemText[i].color = gemCol;
            }

            if (i < deathText.Length)
            {
                deathText[i].text = i < entries.Count ? entries[i].deaths.ToString() : "-";
                Color deathCol = deathText[i].color;
                deathCol.a = i < entries.Count ? 1f : 0.5f;
                deathText[i].color = deathCol;
            }

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
        if (!allowEntry) return;
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
                PlayerPrefs.DeleteKey($"LBGhost_{i}");
                PlayerPrefs.DeleteKey($"LBGems_{i}");
                PlayerPrefs.DeleteKey($"LBDeaths_{i}");
                PlayerPrefs.DeleteKey($"DistLBName_{i}");
                PlayerPrefs.DeleteKey($"DistLBValue_{i}");
                PlayerPrefs.DeleteKey($"DistLBGhost_{i}");
                PlayerPrefs.DeleteKey($"DistLBGems_{i}");
                PlayerPrefs.DeleteKey($"DistLBDeaths_{i}");
            }
            PlayerPrefs.Save();
            entries.Clear();
            RefreshList();
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) enterName();
    }
}
