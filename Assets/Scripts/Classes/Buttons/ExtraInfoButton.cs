using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraInfoButton : MonoBehaviour, IPointerClickHandler {

    [Header("Animation")]
    [SerializeField] Animator clipAnimator;

    [Header("Buttons")]
    [SerializeField] GameObject nextPrefabButton;

    [Header("Events")]
    public GameEvent onGetNextPrefab;

    [SerializeField] Texture2D objectCursor;

    public void OnMouseEnter() {
        Cursor.SetCursor(objectCursor, new Vector2(12, 10), CursorMode.Auto);
        clipAnimator.SetBool("ClipHover", true);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, new Vector2(12, 10), CursorMode.Auto);
        clipAnimator.SetBool("ClipHover", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onGetNextPrefab.Raise(eventData);
        //StartCoroutine(AnimationCoroutine(canvasGroup));
    }

    // IEnumerator AnimationCoroutine(CanvasGroup canvasGroup)
    // {
    //     yield return new WaitForSeconds(3);
    // }
}
