using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animation[] anims;

    void Start()
    {
        anims = GetComponentsInChildren<Animation>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var anim in anims)
        {
            foreach (AnimationState state in anim)
            {
                state.speed = 1;
                anim.Blend(state.name);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var anim in anims)
        {
            foreach (AnimationState state in anim)
            {
                state.speed = -1;
                state.time = state.length;
                anim.Blend(state.name);
            }
        }
    }
}
