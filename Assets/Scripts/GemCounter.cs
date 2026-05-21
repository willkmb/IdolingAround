using TMPro;
using UnityEngine;

public class GemCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gemText;
    [HideInInspector] public int gems;


    public void UpdateGemCounter()
    {
        gems++;
        gemText.text = gems.ToString();
    }
}
