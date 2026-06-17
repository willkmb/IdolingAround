using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] string textContent;
    [SerializeField] GameObject box;
    [SerializeField] GameObject[] otherboxes;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            text.gameObject.GetComponent<Animation>().Play("TutTextIn");
            text.text = textContent;
            foreach (var box in otherboxes)
            {
                SpriteRenderer rend = box.GetComponent<SpriteRenderer>();
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0f);
            }
            if (box == null) return;
            box.GetComponent<Animation>().Play();
        }
    }
}
