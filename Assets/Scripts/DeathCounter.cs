using TMPro;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathText;
     public int deaths;

    public void UpdateDeathCounter()
    {
        deaths++;
        deathText.text = deaths.ToString();
    }
}
