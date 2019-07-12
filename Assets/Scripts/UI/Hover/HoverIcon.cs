using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D icon;
    public Vector2   Offset = new Vector2(16, 16);

    public void OnPointerEnter(PointerEventData eventData) {
        Cursor.SetCursor(icon, Offset, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData) { Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); }
}