using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CentralUIButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]
    [SerializeField] GameObject centralUIButton;

    [Header("Events")]
    public GameEvent onSwitchCentralUIState;

    [SerializeField] Texture2D objectCursor;

    public void OnMouseEnter() {
        Cursor.SetCursor(objectCursor, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSwitchCentralUIState.Raise();
    }
}
