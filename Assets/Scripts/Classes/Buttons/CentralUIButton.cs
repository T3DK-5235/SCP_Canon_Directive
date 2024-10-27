using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CentralUIButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]
    [SerializeField] GameObject centralUIButton;
    [SerializeField] GameObject sitesUIButton;

    [Header("Events")]
    public GameEvent onSwitchCentralUIState;

    [SerializeField] Texture2D objectCursor;

    public void OnMouseEnter() {
        Debug.Log(this.gameObject.name);
        if (this.gameObject == centralUIButton) {
            Debug.Log("this.gameobject does return same as gameobject in field");
        }
        Cursor.SetCursor(objectCursor, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.gameObject == centralUIButton) {
            onSwitchCentralUIState.Raise("top"); 
        } else if (this.gameObject == sitesUIButton) {
            onSwitchCentralUIState.Raise("bottom");
        }
    }
}
