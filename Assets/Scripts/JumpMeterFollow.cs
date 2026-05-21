using UnityEngine;
using UnityEngine.UI;

public class JumpMeterFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Camera cam;

    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }
}