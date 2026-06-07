using UnityEngine;
using UnityEngine.UI;
public class JumpMeterFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Camera cam;
    [SerializeField] float padTop = 40f;

    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        screenPos.y = Mathf.Min(screenPos.y, Screen.height - padTop);
        transform.position = screenPos;
    }
}