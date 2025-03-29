using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator description;
    private bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetBool("isHovering", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.SetBool("isHovering", false);
    }

}
