using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] string textContent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            text.gameObject.GetComponent<Animation>().Play("TutTextIn");
            text.text = textContent;
        }
    }
}
