using Unity.VisualScripting;
using UnityEngine;

public class MenuMove : MonoBehaviour
{
    [SerializeField] float strength = 20f;
    [SerializeField] float smoothing = 8f;
    [SerializeField] float maxOffset = 30f;
    private Vector3 startPos;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        Vector2 mouse = Input.mousePosition;
        Vector2 centered = new Vector2((mouse.x / Screen.width - 0.5f) * 2f,(mouse.y / Screen.height - 0.5f) * 2f);
        Vector3 originalPos = startPos + new Vector3(centered.x, centered.y, 0f) * strength;
        Vector3 target = new Vector3(Mathf.Clamp(originalPos.x, startPos.x - maxOffset, startPos.x + maxOffset),Mathf.Clamp(originalPos.y, startPos.y - maxOffset, startPos.y + maxOffset),0f);
        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, target, smoothing * Time.deltaTime);

        if (Cursor.lockState != CursorLockMode.None)
            Debug.Log("Cursor locked by: " + new System.Diagnostics.StackTrace());
    }
}
