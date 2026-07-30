using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ghostRecorder : MonoBehaviour
{
    public static List<Vector3> positions = new List<Vector3>();
    public static List<Vector3> rotations = new List<Vector3>();
    public static List<float> timestamp = new List<float>();

    private int frames = 0;
    [Range(1f, 20f)]
    [SerializeField]private int frameCount = 5;
    private bool recording = false;
    private float recordingTime = 0f;

    public static string selectedId = "";

    public void startRecording()
    {
        positions.Clear();
        rotations.Clear();
        timestamp.Clear();
        recordingTime = 0f;
        recording = true;
    }

    public void stopRecording()
    {
        recording = false;
        Save();
    }

    private void Save()
    {
        string posString = "";
        string rotString = "";
        string timeString = "";
        float startTime = timestamp[0];

        foreach (Vector3 pos in positions) posString += pos.x + "#" + pos.y + "#" + pos.z + "|";
        foreach (Vector3 rot in rotations) rotString += rot.x + "#" + rot.y + "#" + rot.z + "|";
        foreach (float time in timestamp) timeString += (time - startTime) + "|";

        PlayerPrefs.SetString("ghostPos", posString);
        PlayerPrefs.SetString("ghostRot", rotString);
        PlayerPrefs.SetString("ghostTime", timeString);
        PlayerPrefs.Save();
    }

    public static void SaveGhost(string id)
    {
        string posString = "";
        string rotString = "";
        string timeString = "";
        float startTime = timestamp[0];

        foreach (Vector3 pos in positions) posString += pos.x + "#" + pos.y + "#" + pos.z + "|";
        foreach (Vector3 rot in rotations) rotString += rot.x + "#" + rot.y + "#" + rot.z + "|";
        foreach (float time in timestamp) timeString += (time - startTime) + "|";

        PlayerPrefs.SetString("ghostPos_" + id, posString);
        PlayerPrefs.SetString("ghostRot_" + id, rotString);
        PlayerPrefs.SetString("ghostTime_" + id, timeString);
        PlayerPrefs.Save();
    }

    public static void load()
    {
        positions.Clear();
        rotations.Clear();
        timestamp.Clear();

        if (string.IsNullOrEmpty(selectedId)) return;

        string posKey = "ghostPos";
        string rotKey = "ghostRot";
        string timeKey = "ghostTime";
        if (!string.IsNullOrEmpty(selectedId))
        {
            posKey += "_" + selectedId;
            rotKey += "_" + selectedId;
            timeKey += "_" + selectedId;
            selectedId = "";
        }

        if (!PlayerPrefs.HasKey(posKey) || !PlayerPrefs.HasKey(rotKey) || !PlayerPrefs.HasKey(timeKey)) return;

        string[] posFrames = PlayerPrefs.GetString(posKey).Split('|');
        string[] rotFrames = PlayerPrefs.GetString(rotKey).Split('|');
        string[] timeFrames = PlayerPrefs.GetString(timeKey).Split('|');

        foreach(string pos in posFrames)
        {
            if(string.IsNullOrEmpty(pos)) continue;
            string[] values = pos.Split("#");
            positions.Add(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])));
        }

        foreach(string rot in rotFrames)
        {
            if (string.IsNullOrEmpty(rot)) continue;
            string[] values = rot.Split("#");
            rotations.Add(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])));
        }

        foreach(string time in timeFrames)
        {
            if (string.IsNullOrEmpty(time)) continue;
            timestamp.Add(float.Parse(time));
        }

    }

    private void Update()
    { 
        if(!recording || GhostPauser.paused) return;

        if (!GhostPauser.paused) recordingTime += Time.deltaTime;
        if (GhostPauser.paused) return;

        frames++;
        if(frames >= frameCount)
        {
            positions.Add(transform.position);
            rotations.Add(transform.eulerAngles);
            timestamp.Add(recordingTime);
            frames = 0;
        }
    }
}
