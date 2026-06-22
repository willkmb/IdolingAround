using UnityEngine;
using UnityEditor;

public class RespawnGizmo : MonoBehaviour
{
    [SerializeField] private float length = 0.8f;
    [SerializeField] private float headSize = 0.35f;
    [SerializeField] private Color color = Color.cyan;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * length;

        Gizmos.DrawLine(start, end);

        Vector3 back = -transform.forward;
        Vector3 right = transform.right;
        Vector3 left = Quaternion.AngleAxis(150f, transform.up) * transform.forward;
        Vector3 rightHead = Quaternion.AngleAxis(-150f, transform.up) * transform.forward;

        Gizmos.DrawLine(end, end + left * headSize);
        Gizmos.DrawLine(end, end + rightHead * headSize);
    }
}
