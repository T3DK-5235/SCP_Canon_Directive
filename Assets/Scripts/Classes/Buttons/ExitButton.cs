using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]
    [SerializeField] GameObject exitButton;

    [SerializeField] Texture2D objectCursor;

    public void OnMouseEnter() {
        Cursor.SetCursor(objectCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
