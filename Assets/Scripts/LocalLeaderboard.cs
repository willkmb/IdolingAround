using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class LocalLeaderboard : MonoBehaviour
{
    private TMP_InputField nameInput;
    private List<(string name, float distance)> entries = new List<(string, float)>();
    private int maxEntries = 8;
    private bool hasEntered = false;
    [SerializeField] TextMeshProUGUI[] entryText;
    [SerializeField] MovementScript movement;
    void Start()
    {
        nameInput = GetComponentInChildren<TMP_InputField>();
        nameInput.characterLimit = 4;
        LoadEntries();
        RefreshList();
    }
    public void EnterName()
    {
        if (hasEntered) return;
        string name = nameInput.text;
        if (string.IsNullOrEmpty(name)) return;
        float distance = movement.GetDistance();
        entries.Add((name.ToLower(), distance));
        entries.Sort((a, b) => b.distance.CompareTo(a.distance));
        if (entries.Count > maxEntries)
            entries.RemoveRange(maxEntries, entries.Count - maxEntries);
        SaveEntries();
        nameInput.text = "";
        hasEntered = true;
        RefreshList();
    }
    void SaveEntries()
    {
        PlayerPrefs.SetInt("LeaderBoardCount", entries.Count);
        for (int i = 0; i < entries.Count; i++)
        {
            PlayerPrefs.SetString($"LBName_{i}", entries[i].name);
            PlayerPrefs.SetFloat($"LBDistance_{i}", entries[i].distance);
        }
        PlayerPrefs.Save();
    }
    void LoadEntries()
    {
        entries.Clear();
        int count = PlayerPrefs.GetInt("LeaderBoardCount", 0);
        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString($"LBName_{i}", "");
            float distance = PlayerPrefs.GetFloat($"LBDistance_{i}", 0f);
            entries.Add((name, distance));
        }
        entries.Sort((a, b) => b.distance.CompareTo(a.distance));
    }
    void RefreshList()
    {
        for (int i = 0; i < entryText.Length; i++)
        {
            if (i < entries.Count)
                entryText[i].text = $"{i + 1}. {entries[i].name} - {Mathf.RoundToInt(entries[i].distance)}m";
            else
                entryText[i].text = $"{i + 1}. ----";
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
                PlayerPrefs.DeleteKey($"LBDistance_{i}");
            }
            PlayerPrefs.Save();
            entries.Clear();
            RefreshList();
        }
    }
}