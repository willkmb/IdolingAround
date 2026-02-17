using System.Collections;
using System.Threading;
using UnityEngine;

public class HammerSwing : MonoBehaviour
{
    [SerializeField] Rigidbody HammerRB;
    void Start()
    {
        StartCoroutine(ResetHammer());
    }

    IEnumerator ResetHammer()
    {
        yield return new WaitForSeconds(4);

        float time = 0;
        Quaternion startValue = transform.localRotation;

        while (time < 1)
        {
            HammerRB.angularVelocity = Vector3.zero;
            transform.localRotation = Quaternion.Lerp(startValue, Quaternion.Euler(new Vector3(0,0,75)), time / 1);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 75));
        yield return null;
        StartCoroutine(ResetHammer());
    }
}
