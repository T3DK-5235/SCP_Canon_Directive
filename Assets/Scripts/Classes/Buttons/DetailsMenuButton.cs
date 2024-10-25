using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetailsMenuButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]
    [SerializeField] GameObject SCPsButton;
    [SerializeField] GameObject TalesButton;
    [SerializeField] GameObject CanonsButton;
    [SerializeField] GameObject SeriesButton;
    [SerializeField] GameObject GroupsButton;

    [Header("Events")]
    public GameEvent onSwitchDetailsMenu;

    [SerializeField] Texture2D objectCursor;

    public void OnMouseEnter() {
        Cursor.SetCursor(objectCursor, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string buttonClicked = "";
        if (eventData.pointerPress == SCPsButton) {
            buttonClicked = "SCPs";
        } else if (eventData.pointerPress == TalesButton) {
            buttonClicked = "Tales";
        } else if (eventData.pointerPress == CanonsButton) {
            buttonClicked = "Canons";
        } else if (eventData.pointerPress == SeriesButton) {
            buttonClicked = "Series";
        } else if (eventData.pointerPress == GroupsButton) {
            buttonClicked = "Groups";
        }

        //Debug.Log(buttonClicked);

        onSwitchDetailsMenu.Raise(buttonClicked);
    }
}
