using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class checkProgressScript : MonoBehaviour
{
    [SerializeField] Transform[] wp;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI distText;
    [SerializeField] Transform waypointParent;
    [HideInInspector] public bool ended = false;

    private int nextWp = 1;
    private float[] distToWp;
    [HideInInspector] public float bestDist = 0f;

    void Start()
    {
        if (MenuScript.corridorStart)
        {
            List<Transform> newWp = new List<Transform>();
            for (int i = 0; i < waypointParent.childCount; i++)
            {
                Transform child = waypointParent.GetChild(i);

                if (child.CompareTag("CorridorPoints"))
                {
                    newWp.Add(child);
                }
            }

            wp = newWp.ToArray();
        }
        distToWp = new float[wp.Length];
        for (int i = 1; i < wp.Length; i++)
        {
            distToWp[i] = distToWp[i - 1] + Vector3.Distance(wpGroundPos(wp[i - 1].position), wpGroundPos(wp[i].position));
        }
    }

    void Update()
    {
        if (ended) return;
        Vector3 PrevWp = wpGroundPos(wp[this.nextWp - 1].position);
        Vector3 nextWp = wpGroundPos(wp[this.nextWp].position);
        Vector3 groundPos = wpGroundPos(player.position);

        float segLen = Vector3.Distance(PrevWp, nextWp);
        Vector3 segDir = (nextWp - PrevWp).normalized;

        float distToSeg = Vector3.Dot(groundPos - PrevWp, segDir);
        distToSeg = Mathf.Clamp(distToSeg, 0f, segLen);

        if (distToSeg >= segLen && this.nextWp < wp.Length - 1)
        {
            this.nextWp++;
            return;
        }

        float totalDistance = distToWp[this.nextWp - 1] + distToSeg;
        bestDist = Mathf.Max(bestDist, totalDistance);

        distText.text = "rolled: \n" + bestDist.ToString("F1") + " m";
    }

    Vector3 wpGroundPos(Vector3 worldPosition)
    {
        worldPosition.y = 0f;
        return worldPosition;
    }
}
