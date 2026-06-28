using NUnit.Framework;
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
    private int frameCount = 3;
    private bool recording = false;

    public void startRecording()
    {
        positions.Clear();
        rotations.Clear();
        timestamp.Clear();
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

        foreach(Vector3 pos in positions) posString += pos.x + "#" + pos.y + "#" + pos.z + "|";
        foreach (Vector3 rot in rotations) rotString += rot.x + "#" + rot.y + "#" + rot.z + "|";
        foreach (float time in timestamp) timeString += time + "|";

        PlayerPrefs.SetString("ghostPos", posString);
        PlayerPrefs.SetString("ghostRot", rotString);
        PlayerPrefs.SetString("ghostTime", timeString);
        PlayerPrefs.Save();
    }

    public static void load()
    {
        positions.Clear();
        rotations.Clear();
        timestamp.Clear();

        if (!PlayerPrefs.HasKey("ghostPos") || !PlayerPrefs.HasKey("ghostRot") || !PlayerPrefs.HasKey("ghostTime")) return;

        string[] posFrames = PlayerPrefs.GetString("ghostPos").Split('|');
        string[] rotFrames = PlayerPrefs.GetString("ghostRot").Split('|');
        string[] timeFrames = PlayerPrefs.GetString("ghostTime").Split('|');

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
        if(!recording) return;

        frames++;
        if(frames >= frameCount)
        {
            positions.Add(transform.position);
            rotations.Add(transform.eulerAngles);
            timestamp.Add(Time.time);
            frames = 0;
        }
    }
}
