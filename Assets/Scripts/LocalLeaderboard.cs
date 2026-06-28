using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LocalLeaderboard : MonoBehaviour
{
    private TMP_InputField nameInput;
    private List<(string name, float value)> entries = new List<(string, float)>();
    private int maxEntries = 8;
    private bool hasEntered = false;
    [SerializeField] TextMeshProUGUI[] entryText;
    [SerializeField] ExhibitionVerToggle ex;

    void Start()
    {
        nameInput = GetComponentInChildren<TMP_InputField>();
        nameInput.characterLimit = 4;
        loadEntries();
        RefreshList();
    }

    public void enterName()
    {
        if (hasEntered) return;
        string name = nameInput.text;
        if (string.IsNullOrEmpty(name)) return;

        float value;
        if (ex.ExhibitionMode)
        {
            value = PlayerPrefs.GetFloat("LastRunDist", 0f);
            entries.Add((name.ToLower(), value));
            entries.Sort((a, b) => b.value.CompareTo(a.value));
        }
        else
        {
            value = PlayerPrefs.GetFloat("LastRunTime", 0f);
            entries.Add((name.ToLower(), value));
            entries.Sort((a, b) => a.value.CompareTo(b.value));
        }

        if (entries.Count > maxEntries) entries.RemoveRange(maxEntries, entries.Count - maxEntries);
        saveEntries();
        nameInput.text = "";
        hasEntered = true;
        RefreshList();
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
            }
        }
        else
        {
            PlayerPrefs.SetInt("LeaderBoardCount", entries.Count);
            for (int i = 0; i < entries.Count; i++)
            {
                PlayerPrefs.SetString($"LBName_{i}", entries[i].name);
                PlayerPrefs.SetFloat($"LBTime_{i}", entries[i].value);
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
            for (int i = 0; i < count; i++)entries.Add((PlayerPrefs.GetString($"DistLBName_{i}", ""), PlayerPrefs.GetFloat($"DistLBValue_{i}", 0f)));
        }
        else
        {
            int count = PlayerPrefs.GetInt("LeaderBoardCount", 0);
            for (int i = 0; i < count; i++)entries.Add((PlayerPrefs.GetString($"LBName_{i}", ""), PlayerPrefs.GetFloat($"LBTime_{i}", 0f)));
        }
        RefreshList();
    }

    void RefreshList()
    {
        for (int i = 0; i < entryText.Length; i++)
        {
            if (i < entries.Count)
            {
                if (ex.ExhibitionMode) entryText[i].text = $"{i + 1}. {entries[i].name} - {entries[i].value:F1} m";
                else entryText[i].text = $"{i + 1}. {entries[i].name} - {Mathf.FloorToInt(entries[i].value / 60f):00}:{Mathf.FloorToInt(entries[i].value % 60f):00}";
            }
            else entryText[i].text = $"{i + 1}. ----";
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteKey("HighScore");
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
