using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]
    [SerializeField] GameObject foundationStatButton;
    [SerializeField] GameObject goiStatButton;

    [Header("UI Panels")]
    [SerializeField] GameObject foundationStatScreen;
    [SerializeField] GameObject goiStatScreen;

    //[Header("Events")]
    //public GameEvent onStatScreenChanged;

    [SerializeField] Texture2D objectCursor;

    public void OnMouseEnter() {
        Cursor.SetCursor(objectCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO Check that the proposal can be accepted due to the stats
        //This deals with the button used for accepting or denying the proposal
        if(eventData.pointerPress == foundationStatButton) {
            foundationStatScreen.SetActive(true);
            goiStatScreen.SetActive(false);
            
        } else if (eventData.pointerPress == goiStatButton) {
            goiStatScreen.SetActive(true);
            foundationStatScreen.SetActive(false);
        }
        //StartCoroutine(AnimationCoroutine(canvasGroup));
    }

    // IEnumerator AnimationCoroutine(CanvasGroup canvasGroup)
    // {
    //     yield return new WaitForSeconds(3);
    // }
}
