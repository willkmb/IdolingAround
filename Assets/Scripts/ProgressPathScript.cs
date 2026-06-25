using UnityEngine;
using TMPro;

public class ProgressPathScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Display")]
    [SerializeField] private string prefix = "Rolled ";
    [SerializeField] private string suffix = "m";
    [SerializeField] private bool showAsPercentage = false;
    [SerializeField] private bool smoothProgress = true;

    private float[] cumulativeDistances;
    private float totalLength;
    private int furthestIndex = 0;
    private float furthestSmoothedDist;

    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            enabled = false;
            return;
        }
        BuildDistanceTable();
    }

    private void BuildDistanceTable()
    {
        cumulativeDistances = new float[waypoints.Length];
        cumulativeDistances[0] = 0f;
        for (int i = 1; i < waypoints.Length; i++)
        {
            float segDist = Vector3.Distance(waypoints[i - 1].position, waypoints[i].position);
            cumulativeDistances[i] = cumulativeDistances[i - 1] + segDist;
        }
        totalLength = cumulativeDistances[waypoints.Length - 1];
    }

    private void Update()
    {
        if (player == null || progressText == null) return;

        AdvanceProgress();
        UpdateText();
    }

    private void AdvanceProgress()
    {
        int closestSegment = -1;
        float closestLateralDist = float.MaxValue;
        float candidateDist = furthestSmoothedDist;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector3 a = waypoints[i].position;
            Vector3 b = waypoints[i + 1].position;
            Vector3 ab = b - a;
            float segLenSqr = ab.sqrMagnitude;
            if (segLenSqr < 0.0001f) continue;

            float t = Vector3.Dot(player.position - a, ab) / segLenSqr;
            t = Mathf.Clamp01(t);

            Vector3 closestPoint = a + ab * t;
            float lateralDist = Vector3.Distance(player.position, closestPoint);

            if (lateralDist < closestLateralDist)
            {
                closestLateralDist = lateralDist;
                closestSegment = i;
                float segLen = Mathf.Sqrt(segLenSqr);
                candidateDist = cumulativeDistances[i] + t * segLen;
            }
        }

        if (closestSegment == -1) return;

        furthestSmoothedDist = Mathf.Max(furthestSmoothedDist, candidateDist);

        while (furthestIndex < waypoints.Length - 1 &&
               cumulativeDistances[furthestIndex + 1] <= furthestSmoothedDist)
        {
            furthestIndex++;
        }
    }

    private void UpdateText()
    {
        float dist = smoothProgress ? furthestSmoothedDist : cumulativeDistances[furthestIndex];
        dist = Mathf.Clamp(dist, 0f, totalLength);

        if (showAsPercentage)
        {
            float pct = totalLength > 0f ? (dist / totalLength) * 100f : 0f;
            progressText.text = $"{prefix}{Mathf.RoundToInt(pct)}{suffix}";
        }
        else
        {
            progressText.text = $"{prefix}{dist:F0}{suffix}";
        }
    }

    public void ResetProgress()
    {
        furthestIndex = 0;
        furthestSmoothedDist = 0f;
    }

    public float GetProgressNormalised() =>
        totalLength > 0f ? Mathf.Clamp01(furthestSmoothedDist / totalLength) : 0f;

    public float GetProgressMetres() => furthestSmoothedDist;
}