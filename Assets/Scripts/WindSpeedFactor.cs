using NUnit.Framework.Internal;
using UnityEngine;

public class WindSpeedFactor : MonoBehaviour
{
    [SerializeField] Material[] wind;
    [SerializeField] string property = "_windstrength";

    [Header("Settings")]
    [SerializeField] float minVal = 0f;
    [SerializeField] float maxVal = 1f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float smoothFactor = 5f;

    float curVal;

    void Update()
    {
        float speed = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0f, GetComponent<Rigidbody>().linearVelocity.z).magnitude;
        float target = Mathf.Lerp(minVal, maxVal, Mathf.Clamp01(speed / maxSpeed));
        curVal = Mathf.MoveTowards(curVal, target, smoothFactor * Time.deltaTime);
        Debug.Log(curVal);

        foreach (var wind in wind)
        {
            wind.SetFloat(property, curVal);
        }
    }
}
