using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LocalLeaderboard : MonoBehaviour
{
    private TMP_InputField nameInput;
    private List<(string name, float time)> entries = new List<(string, float)>();
    private int maxEntries = 8;
    private bool hasEntered = false;
    [SerializeField] TextMeshProUGUI[] entryText;

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

        float time = PlayerPrefs.GetFloat("HighScore", 0f);
        entries.Add((name.ToLower(), time));
        entries.Sort((a,b) => a.time.CompareTo(b.time));
        if(entries.Count > maxEntries) entries.RemoveRange(maxEntries, entries.Count - maxEntries);

        saveEntries();
        nameInput.text = "";
        hasEntered = true;
        RefreshList();
    }

    void saveEntries()
    {
        PlayerPrefs.SetInt("LeaderBoardCount", entries.Count);
        for(int i = 0; i < entries.Count; i++)
        {
            PlayerPrefs.SetString($"LBName_{i}", entries[i].name);
            PlayerPrefs.SetFloat($"LBTime_{i}", entries[i].time);
            
        }
        PlayerPrefs.Save();
    }

    void loadEntries()
    {
        entries.Clear();
        int count = PlayerPrefs.GetInt("LeaderBoardCount", 0);
        for(int i = 0;i < count; i++)
        {
            string name = PlayerPrefs.GetString($"LBName_{i}", "");
            float time = PlayerPrefs.GetFloat($"LBTime_{i}", 0f);
            entries.Add((name, time));
        }
        RefreshList();
    }

    void RefreshList()
    {
        for (int i = 0; i < entryText.Length; i++)
        {
            if (i < entries.Count) entryText[i].text = $"{i + 1}. {entries[i].name} - {Mathf.FloorToInt(entries[i].time / 60f):00}:{Mathf.FloorToInt(entries[i].time % 60f):00}";
            else entryText[i].text = $"{i + 1}. ----";
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteKey("HighScore");
            PlayerPrefs.DeleteKey("LeaderBoardCount");
            for (int i = 0; i < maxEntries; i++)
            {
                PlayerPrefs.DeleteKey($"LBName_{i}");
                PlayerPrefs.DeleteKey($"LBTime_{i}");
            }
            PlayerPrefs.Save();
            entries.Clear();
            RefreshList();
            Debug.Log("Cleared. Entry count: " + entries.Count + " Text count: " + entryText.Length);
        }
    }
}
